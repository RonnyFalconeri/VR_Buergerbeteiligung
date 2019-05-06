using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Menu_Script : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            if (false == PhotonNetwork.InLobby)
            {
                Debug.Log("Joined default lobby.");
                PhotonNetwork.JoinLobby();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyMenu");
        } 
    }
}
