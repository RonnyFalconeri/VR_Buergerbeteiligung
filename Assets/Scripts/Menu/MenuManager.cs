using Photon.Pun;
using System;
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
        [Header("Panels")]
        public GameObject panelCreateRoom;
        public GameObject panelLogin;
        public GameObject panelLobby;
        public GameObject panelEditProfile;

        [Header("Lobby Panel")]
        public Button btnSwitchToLogin;
        public Button btnSwitchToCreateRoom;
        public Button btnEditProfile;
        public Button btnCreateRoom_Testing;
        public Button btnReconnect;
        public TMP_Text lblLoggedInInfo;
        public TMP_Text lblUsername;
        public TMP_Text lblConnState;

        [Header("Login Panel")]
        public TMP_InputField inpUsername;
        public TMP_InputField inpPassword;
        public TMP_Text lblLoginError;

        [Header("Edit Profile Panel")]
        public TMP_InputField inpChangeUsername;

        [Header("Create Room Panel")]
        public TMP_InputField inpRoomName;
        public TMP_InputField inpMaxPlayers;
        public Toggle chkLoginRequired;
        public Dropdown dropdownRoomType;
        public Button btnCreateRoom;
        public TMP_Text lblCreateRoomError;

        public AccountManager accountManager;
        public NetworkManager networkManager;

        private static bool alreadyRunning = false;

        // Start is called before the first frame update
        void Start()
        {
            switchToPanel("panelLobby");
            // disable VR for menu view
            XRSettings.enabled = false;
            if ( alreadyRunning )
            {
                // we just returned from a room
                OnServerConnStateChanged(false, PhotonNetwork.IsConnected);

                if ( false == PhotonNetwork.LocalPlayer.NickName.Equals("anonymous") )
                {
                    UserData userdata = new UserData();
                    userdata.isModerator = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"];
                    userdata.name = PhotonNetwork.LocalPlayer.NickName;
                    userdata.avatar = (string)PhotonNetwork.LocalPlayer.CustomProperties["avatar"];
                    OnUserLoggedIn(userdata);
                }
            }
            else
            {
                alreadyRunning = true;
            }
        }

        public void OnClickSwitchToCreateRoom()
        {
            lblCreateRoomError.text = "";
            inpRoomName.text = "";
            switchToPanel("panelCreateRoom");
        }

        public void OnClickSwitchToLogin()
        {
            lblLoginError.text = "";
            inpPassword.text = "";
            switchToPanel("panelLogin");
        }

        public void OnClickSwitchToLobby()
        {
            switchToPanel("panelLobby");
        }

        public void OnClickSwitchToConfig()
        {
            inpChangeUsername.text = accountManager.GetCurrentUserdata().name;
            switchToPanel("panelEditProfile");
        }

        public void switchToPanel(string name)
        {
            panelCreateRoom.SetActive(name.Equals(panelCreateRoom.name));
            panelLobby.SetActive(name.Equals(panelLobby.name));
            panelLogin.SetActive(name.Equals(panelLogin.name));
            panelEditProfile.SetActive(name.Equals(panelEditProfile.name));
        }

        public void OnClickLogin()
        {
            if ( false == accountManager.Login(inpUsername.text, inpPassword.text) )
            {
                lblLoginError.text = "Ungültige Anmeldedaten!";
            }
        }

        public void OnClickLogout()
        {
            accountManager.Logout();
        }

        public void loginForRestrictedRoom(string roomName)
        {
            switchToPanel("panelLogin");
            accountManager.JoinRoomAfterLogin(roomName);
        }

        public void OnUserLoggedIn(UserData userdata)
        {
            // activate/deactive game objects...
            btnSwitchToLogin.gameObject.SetActive(false);
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
            btnSwitchToLogin.gameObject.SetActive(true);
            btnEditProfile.gameObject.SetActive(false);
            lblLoggedInInfo.gameObject.SetActive(false);
            lblUsername.gameObject.SetActive(false);
            btnCreateRoom.gameObject.SetActive(false);

            OnClickSwitchToLobby();
        }

        public void OnClickApplyProfileChanges()
        {
            string newName = inpChangeUsername.text;
            // account manager updates network manager
            accountManager.changeUsername(newName);
            lblUsername.text = newName;
        }

        public void OnClickCreateRoom()
        {
            // get selected room type
            string roomType = dropdownRoomType.options[dropdownRoomType.value].text;
            string roomName = inpRoomName.text;
            int maxPlayers = 0;
            Int32.TryParse(inpMaxPlayers.text, out maxPlayers);
            bool isLoginRequired = chkLoginRequired.isOn;

            networkManager.CreateRoom(roomType, roomName, maxPlayers, isLoginRequired);
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

            btnSwitchToLogin.interactable = isConnected;
            btnCreateRoom.interactable = isConnected;
            btnCreateRoom_Testing.interactable = isConnected; 
        }
    }
}
