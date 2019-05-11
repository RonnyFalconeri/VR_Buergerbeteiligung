using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Menu_Script : MonoBehaviour
    {
        public void OnClickLeaveRoom()
        {
            // leave photon room and load lobby scene
            PhotonNetwork.LeaveRoom();
            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyMenu");
        } 
    }
}
