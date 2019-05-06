using Photon.Pun;
using UnityEngine;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public GameObject avatar;
        public Transform player;
        public Transform playerCamera;
        public VoteMaster Voting;
        private bool Voted = false;

        

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player instantiated.");
            if ( (photonView.IsMine) || (false == PhotonNetwork.IsConnected) )
            {
                player = GameObject.Find("OVRPlayerController").transform;
                playerCamera = player.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");

                // add network with avatar to OVR player and correct positioning
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
            if (Input.GetKey("g"))
            {
                Vote_Yes();

            } else
            if (Input.GetKey("h"))
            {
                Vote_No();

            }

        }

        public void Vote_Yes()
        {
            if (!Voted)
            {
                //Voting.Vote("yes");
                this.photonView.RPC("OnVoted", RpcTarget.AllViaServer, "yes");
                Voted = true;
            }
        }

        public void Vote_No()
        {
            if (!Voted)
            {
                //Voting.Vote("no");
                this.photonView.RPC("OnVoted", RpcTarget.AllViaServer, "no");
                Voted = true;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // update position via network
            if ( stream.IsWriting )
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

        // for test purposes
        [PunRPC]
        void OnVoted(string selection, PhotonMessageInfo info)
        {
            if ( true == (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
            {
                // the photonView.RPC() call is the same as without the info parameter.
                // the info.Sender is the player who called the RPC.
                Voting.Vote(selection);
                Debug.Log(info.Sender.NickName + " voted for " + selection);
            }
        }
    }
}
