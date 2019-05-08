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

            GameObject player = GameObject.Find("OVRPlayerController/NetworkPlayer(Clone)");
            if ( null != player )
            {
                Transform transform = player.transform;
                NetworkPlayer networkPlayer = (NetworkPlayer)transform.GetComponent("NetworkPlayer");
                if (null != networkPlayer )
                {
                    networkPlayer.OnClickStartVoting(topic);
                }
            }
        }

        public void OnClickVoted(string vote)
        {
            GameObject player = GameObject.Find("OVRPlayerController/NetworkPlayer(Clone)");
            if (null != player)
            {
                Transform transform = player.transform;
                NetworkPlayer networkPlayer = (NetworkPlayer)transform.GetComponent("NetworkPlayer");
                if ( null != networkPlayer )
                {
                    networkPlayer.OnClickVoted(vote);
                }
            }
        }
    }
}
