using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public GameObject avatar;

        private Transform player;
        private Transform playerCamera;
        private VoteMaster voteMaster;

        // Start is called before the first frame update
        void Start()
        {
            if ( PhotonNetwork.IsConnected )
            {
                if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
                {
                    // moderator manages voting
                    voteMaster = new VoteMaster();
                }
            }

            // only for own player connect avatar to the controller
            if ( (photonView.IsMine) || (false == PhotonNetwork.IsConnected) )
            {
                // get OVRPlayerController and center camera
                player = GameObject.Find("OVRPlayerController").transform;
                playerCamera = player.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
                // add own player to OVR controller and correct positioning
                this.transform.SetParent(player);
                this.transform.localPosition = new Vector3(0, 0, 0);
                this.transform.localRotation = Quaternion.identity;
                // disable avatar for own player so it doesnt disturb the view
                avatar.SetActive(false);

                if ( 0 == SceneManagerHelper.ActiveSceneName.CompareTo("Presentation") )
                {
                    // in presentation room, disable movement
                    OVRPlayerController controller = (OVRPlayerController)player.GetComponent("OVRPlayerController");
                    if ( null != controller )
                    {
                        controller.EnableLinearMovement = false;
                    }
                }
            }
        }

        public void OnClickVoted(string vote)
        {
            Debug.Log("sending vote to mod now");
            // vote is send to all players, but moderator filters in method
            GameObject mod = GameObject.FindGameObjectWithTag("VoteMaster");
            if ( null != mod )
            {
                Debug.Log("voteMAster found");
                NetworkPlayer networkplayer = (NetworkPlayer)mod.GetComponent("NetworkPlayer");
                if ( null != networkplayer )
                {
                    Debug.Log("transfering vote to votemaster now");
                    networkplayer.photonView.RPC("OnVoted", RpcTarget.AllViaServer, vote);
                }
            }
            ToggleVotingMenu(false, "");
        }

        public void OnClickStartVoting(string topic)
        {
            // called when moderator starts voting
            // remove old voting before
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            Debug.Log("transmit voting to players now");
            voteMaster.Create_New_Voting(topic);
            this.gameObject.tag = "VoteMaster";
            this.photonView.RPC("OnVotingStarted", RpcTarget.AllBufferedViaServer, topic);
        }

        public void OnClickFinishVoting()
        {
            // called when moderator finishes voting
            // remove the vote RPC so new players don't get it anymore
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            this.photonView.RPC("OnVotingFinished", RpcTarget.Others);
            // evaluate voting
            this.gameObject.tag = "";
            voteMaster.Get_Result();
            voteMaster.Save_Result();
        }

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

        [PunRPC]
        public void OnVotingStarted(string request, PhotonMessageInfo info)
        {
            Debug.Log("Neue Abstimmung: " + request);
            // activate menu
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
            Debug.Log("vote received from " + info.Sender.NickName);
            if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
            {
                // only moderator evaluates the votes
                Debug.Log(info.Sender.NickName + " voted for " + selection);
                voteMaster.Vote(selection);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // update position via network
            if (stream.IsWriting)
            {
                // if we are writing, this is our own player
                stream.SendNext(player.position);
                stream.SendNext(player.rotation);
                stream.SendNext(playerCamera.localPosition);
                stream.SendNext(playerCamera.localRotation);
                stream.SendNext(this.gameObject.tag);
            }
            else
            {
                // in reading mode, this refers to the related NetworkPlayer object
                // this is not our own player, but every other player
                this.transform.position = (Vector3)stream.ReceiveNext();
                this.transform.rotation = (Quaternion)stream.ReceiveNext();
                avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
                this.gameObject.tag = (string)stream.ReceiveNext();
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
    }
}
