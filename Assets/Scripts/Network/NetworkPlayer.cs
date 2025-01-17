﻿using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun
    {
        public TMP_Text lblUsername;
        public GameObject avatar;

        bool isMod = false;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player (phview " + this.photonView.ViewID + ") instantiated.");
            if ( 0 == SceneManagerHelper.ActiveSceneName.CompareTo("Presentation") )
            {
                if ( false == isMod )
                {
                    // don't show avatars in presentation (except moderator (set via RPC))
                    avatar.SetActive(false);
                }
            }

            if ( (photonView.IsMine) || (false == PhotonNetwork.IsConnected) )
            {
                // get OVRPlayerController and center camera
                Transform player = GameObject.Find("OVRPlayerController").transform;
                if ( null != player )
                {
                    // add own player to OVR controller and correct positioning
                    this.transform.SetParent(player);
                    this.transform.localPosition = new Vector3(0, 0, 0);
                    this.transform.localRotation = Quaternion.identity;
                }
                
                // disable avatar for own player so it doesnt disturb the view
                avatar.SetActive(false);

                if ( PhotonNetwork.IsConnected )
                {
                    if (false == (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"])
                    {
                        if ((player != null) && (0 == SceneManagerHelper.ActiveSceneName.CompareTo("Presentation")))
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
                    this.photonView.RPC("SetUsername", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
                }
            }
        }

        [PunRPC]
        public void MarkAsModerator()
        {
            isMod = true;
            avatar.SetActive(true);
        }

        [PunRPC]
        public void SetUsername(string username)
        {
            lblUsername.text = username;
        }
    }
}
