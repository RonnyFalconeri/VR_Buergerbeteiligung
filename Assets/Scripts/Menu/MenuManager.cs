using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace VRRoom
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject panelCreateRoom;
        public GameObject panelLogin;
        public GameObject panelLobby;
        public GameObject panelEditProfile;

        public Button btnLogin;
        public Button btnCreateRoom;
        public Button btnEditProfile;
        public Button btnCreateRoom_Testing;
        public Button btnReconnect;

        public TMP_Text lblLoggedInInfo;
        public TMP_Text lblUsername;
        public TMP_Text lblConnState;

        public GameObject objRoomListContent;
        public GameObject objRoomListEntryPrefab;

        public AccountManager accountManager;

        // Start is called before the first frame update
        void Start()
        {
            switchToPanel("panelLobby");
            // disable VR for menu view
            enableVR(false);
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

        public void OnClickSwitchToConfig()
        {
            switchToPanel("panelEditProfile");
        }

        public void switchToPanel(string name)
        {
            panelCreateRoom.SetActive(name.Equals(panelCreateRoom.name));
            panelLobby.SetActive(name.Equals(panelLobby.name));
            panelLogin.SetActive(name.Equals(panelLogin.name));
            panelEditProfile.SetActive(name.Equals(panelEditProfile.name));
        }

        public void loginForRestrictedRoom(string roomName)
        {
            switchToPanel("panelLogin");
            accountManager.JoinRoomAfterLogin(roomName);
        }

        public void OnUserLoggedIn(UserData userdata)
        {
            // activate/deactive game objects...
            btnLogin.gameObject.SetActive(false);
            btnEditProfile.gameObject.SetActive(true);
            lblLoggedInInfo.gameObject.SetActive(true);
            lblUsername.gameObject.SetActive(true);
            if ( userdata.isModerator )
            {
                btnCreateRoom.gameObject.SetActive(true);
            }
            lblUsername.text = userdata.name;
        }

        public void OnUserLoggedOut()
        {
            // activate/deactive game objects...
            btnLogin.gameObject.SetActive(true);
            btnEditProfile.gameObject.SetActive(false);
            lblLoggedInInfo.gameObject.SetActive(false);
            lblUsername.gameObject.SetActive(false);
            btnCreateRoom.gameObject.SetActive(false);

            OnClickSwitchToLobby();
        }

        public void OnServerConnStateChanged(bool isConnecting, bool isConnected)
        {
            if (isConnecting)
            {
                lblConnState.text = "Verbinden...";
                lblConnState.color = Color.green;
                btnReconnect.gameObject.SetActive(false);
            }
            else if (isConnected)
            {
                lblConnState.text = "Verbunden";
                lblConnState.color = Color.green;
                btnReconnect.gameObject.SetActive(false);
            }
            else
            {
                lblConnState.text = "Keine Verbindung!";
                lblConnState.color = Color.red;
                btnReconnect.gameObject.SetActive(true);
            }

            btnLogin.interactable = isConnected;
            btnCreateRoom.interactable = isConnected;
            btnCreateRoom_Testing.interactable = isConnected; 
        }

        public void enableVR(bool enable)
        {
            XRSettings.enabled = enable;
        }

        // just for testing
        private void simulateRooms()
        {
            GameObject entry = Instantiate(objRoomListEntryPrefab);
            entry.transform.SetParent(objRoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            // We have to set z-coordinate explicitly to zero because it somehow has a random value after creation
            // and as a result the component is not visible (because of 2D view)
            entry.transform.localPosition = new Vector3(entry.transform.position.x, entry.transform.position.y, 0f);
            entry.GetComponent<RoomListEntry>().Initialize("test", (byte)5, 20, false, this);
        }
    }
}
