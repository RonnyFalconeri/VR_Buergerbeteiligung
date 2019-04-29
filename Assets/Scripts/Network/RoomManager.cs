using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace VRRoom
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;

        void Start()
        {
            // spawn own player on network (gets positioned in NetworkPlayer.cs)
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
