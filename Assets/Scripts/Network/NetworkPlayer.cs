using Photon.Pun;
using UnityEngine;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public GameObject avatar;
        public Transform player;
        public Transform playerCamera;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player instantiated.");
            if ( photonView.IsMine )
            {
                player = GameObject.Find("OVRPlayerController").transform;
                playerCamera = player.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");

                // add player and avatar to camera and correct positioning
                this.transform.SetParent(playerCamera);
                this.transform.localPosition = new Vector3(0, -1, 0);
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
    }
}
