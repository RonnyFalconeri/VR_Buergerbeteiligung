using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{
    public class VotingManager : MonoBehaviourPun
    {
        public Toggle toggleModMenu;
        public InputField inpTopic;
        private VoteMaster votingSystem;

        void Start()
        {
            votingSystem = new VoteMaster();
            if ( PhotonNetwork.IsConnected )
            {
                if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
                {
                    // activate moderator menu for...moderator
                    toggleModMenu.gameObject.SetActive(true);
                }
            }
        }

        public void OnClickStartVoting()
        {
            string topic = inpTopic.text;

            // remove old voting before
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            votingSystem.Create_New_Voting(topic);
            Debug.Log("transmit voting to players now");
            this.photonView.RPC("OnVotingStarted", RpcTarget.OthersBuffered, topic);
        }

        public void OnClickFinishVoting()
        {
            // remove the vote RPC so new players don't get it anymore
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            this.photonView.RPC("OnVotingFinished", RpcTarget.Others);
            // evaluate voting
            Debug.Log(votingSystem.Get_Result());
            votingSystem.Save_Result();
        }

        public void OnClickVoted(string vote)
        {
            // vote is send to all players, but only moderator evaluates
            this.photonView.RPC("OnVoted", RpcTarget.AllViaServer, vote);
            ToggleVotingMenu(false, "");
        }
        

        [PunRPC]
        public void OnVotingStarted(string request, PhotonMessageInfo info)
        {
            Debug.Log("Neue Abstimmung: " + request);
            ToggleVotingMenu(true, request);
        }

        [PunRPC]
        public void OnVotingFinished(PhotonMessageInfo info)
        {
            Debug.Log("voting finished");
            ToggleVotingMenu(false, "");
        }

        [PunRPC]
        public void OnVoted(string selection, PhotonMessageInfo info)
        {
            if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
            {
                // only moderator evaluates the votes
                Debug.Log(info.Sender.NickName + " voted for " + selection);
                votingSystem.Vote(selection);
            }
        }

        private void ToggleVotingMenu(bool visible, string request)
        {
            GameObject voteMenu = GameObject.Find("OVRPlayerController/Inworld_Vote");
            if (null != voteMenu)
            {
                // trying to enable canvas
                Canvas canvas = (Canvas)voteMenu.GetComponent("Canvas");
                if (null != canvas)
                {
                    canvas.enabled = visible;
                }
                Transform textobj = voteMenu.transform.Find("Text");
                if (null != textobj)
                {
                    Text topic = (Text)textobj.GetComponent("Text");
                    if (null != topic)
                    {
                        topic.text = request;
                    }
                }
            }
        }

        //not used yet
        public void SendRPCforAllCharacters(string rpc, RpcTarget target, string param)
        {
            // Send the RPC for every character in the room
            GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
            foreach (GameObject player in players)
            {
                NetworkPlayer networkplayer = (NetworkPlayer)player.GetComponent("NetworkPlayer");
                networkplayer.photonView.RPC(rpc, target, param);
            }
        }
    }
}
