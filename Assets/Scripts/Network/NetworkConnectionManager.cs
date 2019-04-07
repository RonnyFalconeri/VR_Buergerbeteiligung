using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkConnectionManager : MonoBehaviourPunCallbacks
    {

        public Button btnLogin;

        public bool isConnecting;

        // Start is called before the first frame update
        void Start()
        {
            isConnecting = false;
        }

        // Update is called once per frame
        void Update()
        {
            // disable when connecting
            btnLogin.interactable = !isConnecting;
        }

        public void OnClickLogin()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = "default";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "v1";

            isConnecting = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            isConnecting = false;
            // TODO: notify user..

            Debug.Log(cause);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            isConnecting = false;
            Debug.Log("Connected successfully!");

            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyMenu");
        }
    }
}
