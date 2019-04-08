using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRRoom
{
    public class NetworkRoomManager : MonoBehaviourPunCallbacks
    {

        public Button btnCreateRoom;
        public Button btnJoinRoom;
        public TMP_InputField inpRoomName;
        public Dropdown dropdownRoomType;
        public Dropdown dropdownRoomList;

        bool isJoining;

        // Start is called before the first frame update
        void Start()
        {
            isJoining = false;
        }

        // Update is called once per frame
        void Update()
        {
            btnCreateRoom.interactable = !isJoining;
            btnJoinRoom.interactable = !isJoining;
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            dropdownRoomList.ClearOptions();

            List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
            foreach (RoomInfo room in roomList)
            {
                string entry = room.Name + " | " + "Players: " + room.PlayerCount;
                Dropdown.OptionData option = new Dropdown.OptionData(entry);
                list.Add(option);
            }

            dropdownRoomList.AddOptions(list);
        }

        public void OnClickCreateRoom()
        {
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

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log(cause);

            // TODO: show error
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoginMenu");
        }
    }
}
