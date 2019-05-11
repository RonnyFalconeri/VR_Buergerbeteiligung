using Photon.Pun;
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
        [Header("gui elements")]
        public GameObject objRoomListContent;
        public GameObject objRoomListEntryPrefab;
        public TMP_Text lblCreateRoomError;

        [Header("Other")]
        public MenuManager menuManager;
        public AccountManager accountManager;

        public bool isConnecting;
        public bool isConnected;
        public bool isJoining;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;

        private static bool alreadyRunning = false;

        // Start is called before the first frame update
        void Start()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            isConnecting = false;
            isConnected = false;
            isJoining = false;

            if ( alreadyRunning )
            {
                // room left, so we're back in lobby
                if (false == PhotonNetwork.InLobby)
                {
                    Debug.Log("Joined default lobby.");
                    PhotonNetwork.JoinLobby();
                }
                isConnected = PhotonNetwork.IsConnected;
            }
            else
            {
                alreadyRunning = true;
                // establish server connection
                ConnectToMaster();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ConnectToMaster()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = "anonymous";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "v1";

            isConnecting = true;
            menuManager.OnServerConnStateChanged(isConnecting, isConnected);
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(cause);
            isConnecting = false;
            isConnected = false;
            menuManager.OnServerConnStateChanged(isConnecting, isConnected);
            menuManager.switchToPanel("panelLobby");
            accountManager.Logout();
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnConnectedToMaster()
        {
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

            PhotonNetwork.LocalPlayer.CustomProperties = new ExitGames.Client.Photon.Hashtable();
            UserData userdata = new UserData();
            userdata.name = "anonymous";
            userdata.isModerator = false;
            userdata.avatar = "default";
            updateUserData(userdata);
        }

        public override void OnLeftLobby()
        {
            // lobby is left when joining a room
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        

        public void CreateRoom(string roomType, string roomName, int maxPlayers, bool isLoginRequired)
        {
            RoomOptions roomOptions = new RoomOptions();

            if ( roomName.Equals("") )
            {
                roomName = "Raum ohne Namen";
            }
            if ( maxPlayers <= 0 )
            {
                maxPlayers = 20;
            }

            roomOptions.MaxPlayers = (byte)maxPlayers;

            // create hashtable with room properties
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            // we store room name as property because we will use unique UUID for photon's own room name
            roomOptions.CustomRoomProperties.Add("name", roomName);
            roomOptions.CustomRoomProperties.Add("type", roomType);
            roomOptions.CustomRoomProperties.Add("login", isLoginRequired);

            // set the keys which are available in lobby
            roomOptions.CustomRoomPropertiesForLobby = new string[3];
            roomOptions.CustomRoomPropertiesForLobby[0] = "name";
            roomOptions.CustomRoomPropertiesForLobby[1] = "type";
            roomOptions.CustomRoomPropertiesForLobby[2] = "login";

            PhotonNetwork.CreateRoom("", roomOptions);
        }

        // This gets only called after creating a room, when just joining the scene is automatically synced.
        public override void OnJoinedRoom()
        {
            isJoining = false;
            Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name + " (name:" + PhotonNetwork.CurrentRoom.CustomProperties["name"] + ") | Members online: " + PhotonNetwork.CurrentRoom.PlayerCount);

            // get room type and load related scene
            string roomType = (string)PhotonNetwork.CurrentRoom.CustomProperties["type"];
            switch (roomType)
            {
            case "Präsentationsraum":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Presentation");
                break;
            case "Besprechungsraum":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Meeting");
                break;
            case "Foyer":
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
            Debug.Log(message);
            isJoining = false;
            lblCreateRoomError.text = "Fehler beim Erstellen des Raumes.";
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            isJoining = false;
        }

        public void updateUserData(UserData userdata)
        {
            PhotonNetwork.LocalPlayer.NickName = userdata.name;
            if ( true == PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isMod") )
            {
                PhotonNetwork.LocalPlayer.CustomProperties["isMod"] = userdata.isModerator;
            }
            else
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Add("isMod", userdata.isModerator);
            }
            if ( true == PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("avatar") )
            {
                PhotonNetwork.LocalPlayer.CustomProperties["avatar"] = userdata.avatar;
            }
            else
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Add("avatar", userdata.avatar);
            }     
        }

        public void OnUserLoggedOut()
        {
            PhotonNetwork.LocalPlayer.NickName = "Anonymous";
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
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
            // Iterate cached list and create one line per room via prefab entry
            foreach ( RoomInfo roomInfo in cachedRoomList.Values )
            {
                GameObject entry = Instantiate(objRoomListEntryPrefab);
                entry.transform.SetParent(objRoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                // We have to set z-coordinate explicitly to zero because it somehow has a random value after creation
                // and as a result the component is not visible (because of 2D view)
                entry.transform.localPosition = new Vector3(entry.transform.position.x, entry.transform.position.y, 0f);

                // Get room info and set roomlist entry accordingly
                bool isLoginNeeded = (bool)roomInfo.CustomProperties["login"];
                bool showLogin      = (isLoginNeeded && (false == accountManager.IsLoggedIn()));
                string roomName     = (string)roomInfo.CustomProperties["name"];
                string internalName = roomInfo.Name;

                entry.GetComponent<VRRoom.RoomListEntry>().Initialize(roomName, internalName, (byte)roomInfo.PlayerCount, roomInfo.MaxPlayers, showLogin, menuManager);

                roomListEntries.Add(roomInfo.Name, entry);
            }
        }
    }
}
