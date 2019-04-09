using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("Join Room Panel")]
        public GameObject panelJoinRoom;
        public GameObject objRoomListContent;
        //public GameObject RoomListEntryPrefab;
        public Button btnSwitchToLogin;
        public Button btnSwitchToCreate;
        public Button btnReconnect;
        public TMP_Text txtConnectionStatus;

        [Header("Create Room Panel")]
        public GameObject panelCreateRoom;
        public TMP_InputField inpRoomName;
        public Dropdown dropdownRoomType;
        public Button btnCreateRoom;

        [Header("Login Panel")]
        public GameObject panelLogin;
        
        public bool isConnecting;
        public bool isConnected;
        public bool isJoining;
        public bool isLoggedIn;

        // Start is called before the first frame update
        void Start()
        {
            switchToPanel("panelLobby");
            isConnecting = false;
            isConnected = false;
            isJoining = false;
            isLoggedIn = false;
            // establish server connection
            ConnectToMaster();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnClickSwitchToCreateRoom()
        {
            switchToPanel("panelCreateRoom");
        }

        public void OnClickSwitchToLogin()
        {
            switchToPanel("panelLogin");
        }

        public void OnClickSwitchToLobby()
        {
            switchToPanel("panelLobby");
        }

        public void ConnectToMaster()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = "default";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "v1";

            isConnecting = true;
            updateConnectionStateGUI();
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            isConnecting = false;
            isConnected = false;
            updateConnectionStateGUI();
            Debug.Log(cause);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            isConnecting = false;
            isConnected = true;
            updateConnectionStateGUI();
            Debug.Log("Connected to master server successfully!");

            // join default lobby to receive room list
            if ( false == PhotonNetwork.InLobby)
            {
                Debug.Log("Joined default lobby.");
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("roomlistupdate");
            foreach (RoomInfo room in roomList)
            {
                Debug.Log(room.Name);
                string entry = room.Name + " | " + "Players: " + room.PlayerCount;
            }
        }


        public void OnClickCreateRoom()
        {
            //RoomOptions roomOptions = new RoomOptions();
            //roomOptions.CustomRoomPropertiesForLobby = { "map", "ai" };
            //roomOptions.CustomRoomProperties = new Hashtable() { { "map", 1 } };
            //roomOptions.MaxPlayers = expectedMaxPlayers;
            string[] properties = new string[1];

            // get selected room type
            string roomType = dropdownRoomType.options[dropdownRoomType.value].text;

            switch ( roomType )
            {
            case "Präsentationsraum":
                properties[0] = "P";
                break;
            case "Besprechungsraum":
                properties[0] = "B";
                break;
            case "Foyer":
                properties[0] = "F";
                break;
            default:
                    break;
            }

            // TODO check if roomname exists
            PhotonNetwork.CreateRoom(inpRoomName.text, new RoomOptions{ MaxPlayers = 20, CustomRoomPropertiesForLobby = properties});
        }

        public void OnClickJoinRoom()
        {
            isJoining = true;
            PhotonNetwork.JoinRoom("test");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            isJoining = false;
            Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name + " | Master: " + PhotonNetwork.IsMasterClient + " | Members online: " + PhotonNetwork.CurrentRoom.PlayerCount);

            // get room type and load related scene
            string roomType = PhotonNetwork.CurrentRoom.PropertiesListedInLobby[0];
            switch (roomType)
            {
            case "P":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Presentation");
                break;
            case "B":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Meeting");
                break;
            case "F":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Foyer");
                break;
            default:
                Debug.Log("Error: Unknown Room type '" + roomType);
                PhotonNetwork.LeaveRoom();
                break;
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log(message);
            isJoining = false;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log(message);
            isJoining = false;
        }

        public void updateConnectionStateGUI()
        {
            if ( isConnecting )
            {
                txtConnectionStatus.text = "Verbinden...";
                txtConnectionStatus.color = Color.green;
                btnReconnect.gameObject.SetActive(false);
            }
            else if ( isConnected )
            {
                txtConnectionStatus.text = "Verbunden";
                txtConnectionStatus.color = Color.green;
                btnReconnect.gameObject.SetActive(false);
            }
            else
            {
                txtConnectionStatus.text = "Keine Verbindung!";
                txtConnectionStatus.color = Color.red;
                btnReconnect.gameObject.SetActive(true);
            }

            btnSwitchToLogin.interactable = isConnected;
            btnSwitchToCreate.interactable = isConnected;
            btnSwitchToLogin.gameObject.SetActive(!isLoggedIn);
        }

        public void switchToPanel(string name)
        {
            panelCreateRoom.SetActive(name.Equals(panelCreateRoom.name));
            panelJoinRoom.SetActive(name.Equals(panelJoinRoom.name));
            panelLogin.SetActive(name.Equals(panelLogin.name));
        }
    }
}
