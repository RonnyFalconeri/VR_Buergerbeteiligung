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
        public Button btnJoinLoginRoomButton;

        private string roomName;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers, bool isLoginNeeded)
        {
            roomName = name;
            // Set GUI
            lblRoomNameText.text = name;
            lblRoomPlayersText.text = currentPlayers + " / " + maxPlayers;
            if ( false == isLoginNeeded )
            {
                btnJoinLoginRoomButton.onClick.AddListener(() =>
                {
                    if (PhotonNetwork.InLobby)
                    {
                        PhotonNetwork.LeaveLobby();
                    }

                    PhotonNetwork.JoinRoom(roomName);
                });
            }
            else
            {

            }
        }
    }
}
