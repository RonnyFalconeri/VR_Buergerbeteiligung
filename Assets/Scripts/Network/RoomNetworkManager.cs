using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR;

namespace VRRoom
{
    public class RoomNetworkManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;

        void Start()
        {
            if ( PhotonNetwork.InLobby )
            {
                // Leave Lobby to not receive unnecessary room updates
                PhotonNetwork.LeaveLobby();
            }

            // spawn own player on network (gets positioned in NetworkPlayer.cs)
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
            // go back to lobby
            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyMenu");
        }
    }
}
