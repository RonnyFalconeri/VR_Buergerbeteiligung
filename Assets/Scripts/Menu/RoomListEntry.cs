using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace VRRoom
{
    public class RoomListEntry : MonoBehaviour
    {
        public TMP_Text lblRoomNameText;
        public TMP_Text lblRoomPlayersText;
        public TMP_Text lblButtonText;
        public Button btnJoinLoginRoomButton;

        private string roomName;
        private string internalRoomName;
        private MenuManager menuManager;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void Initialize(string name, string internalName, byte currentPlayers, byte maxPlayers, bool isLoginNeeded, MenuManager menuMng)
        {
            roomName = name;
            internalRoomName = internalName;
            this.menuManager = menuMng;

            // Set GUI
            Debug.Log("received roomname" + name);
            lblRoomNameText.text = name;
            lblRoomPlayersText.text = currentPlayers + " / " + maxPlayers;
            if ( false == isLoginNeeded )
            {
                // no login needed for room, so directly display 'join' button
                btnJoinLoginRoomButton.onClick.AddListener(() =>
                {
                    Debug.Log("joining" + roomName);
                    PhotonNetwork.JoinRoom(internalName);
                });
            }
            else
            {
                // room is only for logged in users
                lblButtonText.text = "Login";
                btnJoinLoginRoomButton.onClick.AddListener(() =>
                {
                    Debug.Log("login in for room " + roomName);
                    menuManager.loginForRestrictedRoom(internalRoomName);
                });
            }
        }
    }
}
