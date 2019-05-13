using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace VRRoom
{
    public class VotingManager : MonoBehaviourPun
    {
        public GameObject startButton;
        public GameObject startButtonvr;
        public GameObject stopButton;
        public GameObject stopButtonvr;
        public Toggle toggleModMenu;
        public Toggle toggleModMenuvr;
        public InputField inpTopic;
        public InputField inpTopicvr;
        private VoteMaster votingSystem;

        void Start()
        {
            votingSystem = new VoteMaster();
            if ( PhotonNetwork.IsConnected )
            {
                if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
                {
                    if (XRDevice.isPresent)
                    {
                        // activate moderator menu for...moderator
                        toggleModMenuvr.gameObject.SetActive(true);
                    }
                    else
                    {
                        // activate moderator menu for...moderator
                        toggleModMenu.gameObject.SetActive(true);
                    }    
                }
            }
            toggleButtons(false);
        }

        public void OnClickStartVoting()
        {
            string topic = "";

            if (XRDevice.isPresent)
            {
                topic = inpTopicvr.text;
            }
            else
            {
                topic = inpTopic.text;
            }

            // remove old voting before
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            votingSystem.Create_New_Voting(topic);
            Debug.Log("transmit voting to players now");
            this.photonView.RPC("OnVotingStarted", RpcTarget.OthersBuffered, topic);
            toggleButtons(true);
        }

        public void OnClickFinishVoting()
        {
            // remove the vote RPC so new players don't get it anymore
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            this.photonView.RPC("OnVotingFinished", RpcTarget.Others);
            toggleButtons(false);

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

        public void toggleButtons(bool started)
        {
            if (XRDevice.isPresent)
            {
                startButtonvr.SetActive(!started);
                stopButtonvr.SetActive(started);
            }
            else
            {
                startButton.SetActive(!started);
                stopButton.SetActive(started);
            }
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
            if ( XRDevice.isPresent )
            {
                voteMenu = GameObject.Find("OVRPlayerController/Inworld_Vote_vr");
            }
            else
            {
                voteMenu = GameObject.Find("OVRPlayerController/Inworld_Vote");
            }

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
