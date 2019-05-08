using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{
    public class Player_Voting_Handler : MonoBehaviour
    {
        public Toggle toggleModMenu;
        public InputField Field;

        void Start()
        {
            if ( PhotonNetwork.IsConnected )
            {
                if ( (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"] )
                {
                    toggleModMenu.gameObject.SetActive(true);
                }
            }
        }

        public void StartVoting()
        {
            string topic = Field.text;
            Debug.Log("Starting vote" + topic);

            Transform controller = GameObject.Find("OVRPlayerController").transform;
            Transform player = controller.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/NetworkPlayer");
            
            if ( null != player )
            {
                NetworkPlayer networkPlayer = (NetworkPlayer)player.GetComponent("NetworkPlayer");
                if (null != networkPlayer )
                {
                    networkPlayer.OnClickStartVoting(topic);
                }
            }
        }

        public void OnClickVoted(string vote)
        {
            Transform controller = GameObject.Find("OVRPlayerController").transform;
            Transform player = controller.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/NetworkPlayer");

            if ( null != player )
            {
                NetworkPlayer networkPlayer = (NetworkPlayer)player.GetComponent("NetworkPlayer");
                if ( null != networkPlayer )
                {
                    networkPlayer.OnClickVoted(vote);
                }
            }
        }
    }
}
