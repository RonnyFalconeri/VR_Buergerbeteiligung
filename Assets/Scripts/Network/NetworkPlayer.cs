﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        public GameObject avatar;
        private Transform player;
        private Transform playerCamera;

        bool isMod = false;

        // Start is called before the first frame update
        void Start()
        {
            if ( 0 == SceneManagerHelper.ActiveSceneName.CompareTo("Presentation") )
            {
                // don't show avatars in presentation (but moderator avatar gets reenabled)
                avatar.SetActive(false);
            }

            if ( photonView.IsMine )
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

                if ( false == (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"])
                {
                    if ( 0 == SceneManagerHelper.ActiveSceneName.CompareTo("Presentation") )
                    {
                        // in presentation room, disable movement for all non mods
                        OVRPlayerController controller = (OVRPlayerController)player.GetComponent("OVRPlayerController");
                        if (null != controller)
                        {
                            controller.EnableLinearMovement = false;
                        }
                    }
                }
                else
                {
                    // notify others about moderator state
                    isMod = true;
                    this.photonView.RPC("MarkAsModerator", RpcTarget.OthersBuffered);
                }
            }
        }

        [PunRPC]
        public void MarkAsModerator()
        {
            isMod = true;
            avatar.SetActive(true);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // update position via network
            if (stream.IsWriting)
            {
                // if we are writing, this is our own player
                //stream.SendNext(player.position);
                //stream.SendNext(player.rotation);
                //stream.SendNext(playerCamera.localPosition);
                //stream.SendNext(playerCamera.localRotation);
            }
            else
            {
                // in reading mode, this refers to the related NetworkPlayer object
                // this is not our own player, but every other player
                //this.transform.position = (Vector3)stream.ReceiveNext();
                //this.transform.rotation = (Quaternion)stream.ReceiveNext();
                //avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
                //avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
