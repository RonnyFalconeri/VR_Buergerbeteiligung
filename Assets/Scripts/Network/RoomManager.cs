using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace VRRoom
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public NetworkPlayer playerPrefab;
        public GameObject spawnPoint;

        [HideInInspector]
        public NetworkPlayer localPlayer;

        Vector3 defaultPos;
        Quaternion defaultRot;

        void Start()
        {
            // Room entered first time
            defaultPos = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
            Debug.Log("spawnpoint:" + defaultPos);
            defaultRot = new Quaternion(spawnPoint.transform.rotation.x, spawnPoint.transform.rotation.y, spawnPoint.transform.rotation.z, spawnPoint.transform.rotation.w);
            NetworkPlayer.RefreshInstance(ref localPlayer, playerPrefab, defaultPos, defaultRot);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player photonPlayer)
        {
            // instantiate the player (don't have to be our own local player)
            Debug.Log("New player entered room");
            NetworkPlayer.RefreshInstance(ref localPlayer, playerPrefab, defaultPos, defaultRot);
        }

    }
}
