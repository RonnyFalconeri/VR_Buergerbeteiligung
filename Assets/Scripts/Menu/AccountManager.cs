using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRRoom
{
    public struct UserData
    {
        public string name;
        public string avatar;
        public bool isModerator;
    }

    public class AccountManager : MonoBehaviour
    {
        [Header("GUI elements")]
        public TMP_InputField inpUsername;
        public TMP_InputField inpPassword;
        public TMP_Text lblLoginError;

        [Header("Other")]
        public MenuManager menuManager;
        public NetworkManager networkManager;

        private string roomNameAfterLogin;
        private bool isLoggedIn;

        private UserData userdata;

        // Start is called before the first frame update
        void Start()
        {
            roomNameAfterLogin = "";
            isLoggedIn = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void JoinRoomAfterLogin(string roomname)
        {
            // the given room shall directly be joined after login
            roomNameAfterLogin = roomname;
        }

        public void OnClickBackToLobby()
        {
            // forget eventually set roomname
            roomNameAfterLogin = "";
            menuManager.OnClickSwitchToLobby();
        }

        public void OnClickLogin()
        {
            lblLoginError.text = "";

            // This should of course call a webserver for validating account data and receiving user data...
            if ( simulateLoginRequest(inpUsername.text, inpPassword.text, ref userdata) )
            {
                isLoggedIn = true;
                // publish the user data
                networkManager.updateUserData(userdata);
                menuManager.OnUserLoggedIn(userdata);

                if ( 0 < roomNameAfterLogin.Length )
                {
                    // directly join preset room
                    PhotonNetwork.JoinRoom(roomNameAfterLogin);
                }
                else
                {
                    // go back to lobby
                    menuManager.OnClickSwitchToLobby();
                }
            }
            else
            {
                lblLoginError.text = "Ungültige Anmeldedaten!";
            }
        }

        private bool simulateLoginRequest(string username, string password, ref UserData userdata)
        {
            // we simulate a webserver here which accesses the user database
            if ( (0 < username.Length) || (0 < password.Length) )
            {
                // for demo purposes every non-empty user/pw is accepted
                userdata = new UserData();
                userdata.name = username;
                userdata.avatar = "default";
                if ( (username.Equals("Moderator123")) && (password.Equals("moderator")) )
                {
                    // plz don't hack this safe account
                    userdata.avatar = "moderator";
                    userdata.isModerator = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnClickLogout()
        {
            menuManager.OnUserLoggedOut();
            networkManager.OnUserLoggedOut();
        }

        public bool IsLoggedIn()
        {
            return isLoggedIn;
        }
    }
}
