using Photon.Pun;
using UnityEngine;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public GameObject avatar;

        private Transform player;
        private Transform playerCamera;
        private VoteMaster voteMaster;
        private bool votingPossible = false;

        private bool isModerator = false;

        // Start is called before the first frame update
        void Start()
        {
            if ( PhotonNetwork.IsConnected )
            {
                isModerator = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"];
                if (isModerator)
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

        void Update()
        {
            // check for voting
            if ( true == votingPossible )
            {
                if ( Input.GetKey("g") )
                {
                    SendVoteToModerator("yes");
                }
                else if ( Input.GetKey("h") )
                {
                    SendVoteToModerator("no");
                }
            }
            if ( Input.GetKey("v") )
            {
                // remove old votings
                // start rpc for new voting
                PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
                this.photonView.RPC("OnVotingStarted", RpcTarget.AllBufferedViaServer, "Seid ihr dafür?");
            }
        }
        
        public void SendVoteToModerator(string vote)
        {
            if ( votingPossible )
            {
                votingPossible = false;
                // vote is send to all players, but moderator filters in method
                this.photonView.RPC("OnVoted", RpcTarget.AllViaServer, vote);
            }
        }

        [PunRPC]
        public void OnVotingStarted(string request)
        {
            Debug.Log("Neue Abstimmung: " + request + "\r\n" + "'g' für ja, 'h' für nein.");
            votingPossible = true;
        }

        [PunRPC]
        public void OnVoted(string selection, PhotonMessageInfo info)
        {
            if ( isModerator )
            {
                // only moderator evaluates the votes
                Debug.Log(info.Sender.NickName + " voted for " + selection);
                voteMaster.Vote(selection);
                
                if ( voteMaster.Get_Voter_Count() >= PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    // everybody voted, so the voting is over.
                    voteMaster.Get_Result();
                }
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
            }
            else
            {
                // in reading mode, this refers to the related NetworkPlayer object
                // this is not our own player, but every other player
                this.transform.position = (Vector3)stream.ReceiveNext();
                this.transform.rotation = (Quaternion)stream.ReceiveNext();
                avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
