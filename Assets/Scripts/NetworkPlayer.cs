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

        public static void RefreshInstance(ref NetworkPlayer player, NetworkPlayer prefab)
        {
            var position = Vector3.zero;
            var rotation = Quaternion.identity;

            if( null != player )
            {
                position = player.transform.position;
                rotation = player.transform.rotation;
                PhotonNetwork.Destroy(player.gameObject);
            }
            Debug.Log(prefab.gameObject.name);
            player = PhotonNetwork.Instantiate(prefab.gameObject.name, position, rotation).GetComponent<NetworkPlayer>();
            Debug.Log(player);
        }
    }
}
