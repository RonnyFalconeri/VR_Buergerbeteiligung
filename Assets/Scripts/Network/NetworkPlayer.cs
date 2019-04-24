using Photon.Pun;
using UnityEngine;

namespace VRRoom
{
    public class NetworkPlayer : MonoBehaviourPun
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player instantiated.");

            if (photonView.IsMine)
            {
            }
        }

        public static void RefreshInstance(ref NetworkPlayer localPlayer, NetworkPlayer prefab, Vector3 defaultPos, Quaternion defaultRot)
        {
            var position = defaultPos;
            var rotation = defaultRot;

            if( null != localPlayer)
            {
                position = localPlayer.transform.position;
                rotation = localPlayer.transform.rotation;
                // destroy the old player and instantly recreate it
                PhotonNetwork.Destroy(localPlayer.gameObject);
            }
            Debug.Log("name of the prefab: " + prefab.gameObject.name);
            localPlayer = PhotonNetwork.Instantiate(prefab.gameObject.name, position, rotation).GetComponent<NetworkPlayer>();
            Debug.Log("validation of created player:" + localPlayer.transform.position);
        }
    }
}
