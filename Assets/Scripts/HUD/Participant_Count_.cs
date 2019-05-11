using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
namespace VRRoom
{
    public class Participant_Count_ : MonoBehaviourPunCallbacks
    {
        public Text participant_count;

        public void Start()
        {
            int count = 0;
            if ( PhotonNetwork.IsConnected )
            {
                count = PhotonNetwork.CurrentRoom.PlayerCount;
            }
            participant_count.text = "Teilnehmer: " + count;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            // update player count
            participant_count.text = "Teilnehmer: " + PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            // update player count
            participant_count.text = "Teilnehmer: " + PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

}