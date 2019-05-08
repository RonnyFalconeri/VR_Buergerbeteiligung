﻿using Photon.Pun;
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

            GameObject player = GameObject.Find("OVRPlayerController/NetworkPlayer(Clone)");
            if ( null != player )
            {
                Debug.Log("player found");
                Transform transform = player.transform;
                NetworkPlayer networkPlayer = (NetworkPlayer)transform.GetComponent("NetworkPlayer");
                if (null != networkPlayer )
                {
                    Debug.Log("playernetwork found");
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
                Debug.Log("click will now be transfered, voting enabled:" + networkPlayer.votingPossible);
                if ( null != networkPlayer )
                {
                    networkPlayer.OnClickVoted(vote);
                }
            }
        }
    }
}
