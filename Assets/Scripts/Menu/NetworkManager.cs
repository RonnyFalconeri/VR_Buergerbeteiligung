﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace VRRoom
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("Join Room Panel")]
        public GameObject objRoomListContent;
        public GameObject objRoomListEntryPrefab;

        [Header("Create Room Panel")]
        public TMP_InputField inpRoomName;
        public Dropdown dropdownRoomType;
        public Button btnCreateRoom;

        [Header("Other")]
        public MenuManager menuManager;

        public bool isConnecting;
        public bool isConnected;
        public bool isJoining;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;

        // Start is called before the first frame update
        void Start()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            isConnecting = false;
            isConnected = false;
            isJoining = false;

            // establish server connection
            ConnectToMaster();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ConnectToMaster()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = "default";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "v1";

            isConnecting = true;
            menuManager.OnServerConnStateChanged(isConnecting, isConnected);
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log(cause);

            isConnecting = false;
            isConnected = false;
            menuManager.OnServerConnStateChanged(isConnecting, isConnected);
            menuManager.switchToPanel("panelLobby");
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            isConnecting = false;
            isConnected = true;
            menuManager.OnServerConnStateChanged(isConnecting, isConnected);
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

            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
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

            // TODO check if roomname already exists
            PhotonNetwork.CreateRoom(inpRoomName.text, new RoomOptions{ MaxPlayers = 20, CustomRoomPropertiesForLobby = properties});
        }

        // join room is called from RoomListEntry.cs
        public override void OnJoinedRoom()
        {
            bool roomEntered = false;
            isJoining = false;
            Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name + " | Master: " + PhotonNetwork.IsMasterClient + " | Members online: " + PhotonNetwork.CurrentRoom.PlayerCount);

            // get room type and load related scene
            string roomType = PhotonNetwork.CurrentRoom.PropertiesListedInLobby[0];
            switch (roomType)
            {
            case "P":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Presentation");
                roomEntered = true;
                break;
            case "B":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Meeting");
                roomEntered = true;
                break;
            case "F":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Foyer");
                roomEntered = true;
                break;
            default:
                Debug.Log("Error: Unknown Room type '" + roomType);
                PhotonNetwork.LeaveRoom();
                break;
            }      

            if ( roomEntered )
            {
                menuManager.enableVR(true);
                if ( PhotonNetwork.InLobby )
                {
                    // Leave Lobby to not receive unnecessary room updates
                    PhotonNetwork.LeaveLobby();
                }
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            isJoining = false;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            isJoining = false;
        }

        public void updateUserData(UserData userdata)
        {
            PhotonNetwork.LocalPlayer.NickName = userdata.name;
        }

        public void OnUserLoggedOut()
        {

        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach ( RoomInfo roomInfo in roomList )
            {
                Debug.Log("room found: " + roomInfo.Name + " | " + "Players: " + roomInfo.PlayerCount);
                if ( (false == roomInfo.IsOpen) || (false == roomInfo.IsVisible) || (roomInfo.RemovedFromList) )
                {
                    // room must not be in cached list (anymore)
                    if ( cachedRoomList.ContainsKey(roomInfo.Name) )
                    {
                        cachedRoomList.Remove(roomInfo.Name);
                    }
                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(roomInfo.Name))
                {
                    cachedRoomList[roomInfo.Name] = roomInfo;
                }
                // Add new room
                else
                {
                    cachedRoomList.Add(roomInfo.Name, roomInfo);
                }
            }
        }

        private void ClearRoomListView()
        {
            Debug.Log("clear room list");
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }
            roomListEntries.Clear();
        }

        private void UpdateRoomListView()
        {
            bool isLoginNeeded = true;
            // Iterate cached list and create one line per room via prefab entry
            foreach ( RoomInfo roomInfo in cachedRoomList.Values )
            {
                GameObject entry = Instantiate(objRoomListEntryPrefab);
                entry.transform.SetParent(objRoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                // We have to set z-coordinate explicitly to zero because it somehow has a random value after creation
                // and as a result the component is not visible (because of 2D view)
                entry.transform.localPosition = new Vector3(entry.transform.position.x, entry.transform.position.y, 0f);
                entry.GetComponent<VRRoom.RoomListEntry>().Initialize(roomInfo.Name, (byte)roomInfo.PlayerCount, roomInfo.MaxPlayers, isLoginNeeded, menuManager);

                roomListEntries.Add(roomInfo.Name, entry);
            }
        }
    }
}
