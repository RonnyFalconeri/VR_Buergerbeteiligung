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
        [Header("Other")]
        public MenuManager menuManager;
        public NetworkManager networkManager;

        private string roomNameAfterLogin;
        private bool isLoggedIn;

        private UserData userdata;

        private static bool alreadyRunning = false;

        // Start is called before the first frame update
        void Start()
        {
            roomNameAfterLogin = "";
            userdata = new UserData();
            isLoggedIn = false;

            if (alreadyRunning)
            {
                // we just returned from a room
                if ( false == PhotonNetwork.LocalPlayer.NickName.Equals("anonymous") )
                {
                    isLoggedIn = true;
                    userdata.isModerator = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isMod"];
                    userdata.name = PhotonNetwork.LocalPlayer.NickName;
                    userdata.avatar = (string)PhotonNetwork.LocalPlayer.CustomProperties["avatar"];
                }
            }
            else
            {
                alreadyRunning = true;
            }
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

        public bool Login(string username, string password)
        {
            // This should of course call a webserver for validating account data and receiving user data...
            if ( simulateLoginRequest(username, password, ref userdata) )
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
                
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool simulateLoginRequest(string username, string password, ref UserData userdata)
        {
            // we simulate a webserver here which accesses the user database
            if ( (0 < username.Length) && (0 < password.Length) )
            {
                // for demo purposes every non-empty user/pw is accepted
                userdata = new UserData();
                userdata.name = username;
                userdata.avatar = "default";
                userdata.isModerator = false;
                if ( (username.StartsWith("mod")) && (password.Equals("moderator")) )
                {
                    userdata.avatar = "moderator";
                    userdata.name = "[mod]" + username.Substring(3);
                    userdata.isModerator = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logout()
        {
            userdata = new UserData();
            userdata.name = "";
            userdata.avatar = "default";
            userdata.isModerator = false;
            isLoggedIn = false;

            menuManager.OnUserLoggedOut();
            networkManager.OnUserLoggedOut();
        }

        public void changeUsername(string username)
        {
            userdata.name = username;
            networkManager.updateUserData(userdata);
        }

        public bool IsLoggedIn()
        {
            return isLoggedIn;
        }

        public UserData GetCurrentUserdata()
        {
            return userdata;
        }
    }
}
