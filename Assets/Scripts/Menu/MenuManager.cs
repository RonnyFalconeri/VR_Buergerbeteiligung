using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRRoom
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject panelCreateRoom;
        public GameObject panelLogin;
        public GameObject panelLobby;

        public GameObject objRoomListContent;
        public GameObject objRoomListEntryPrefab;

        // Start is called before the first frame update
        void Start()
        {
            switchToPanel("panelLobby");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickSwitchToCreateRoom()
        {
            switchToPanel("panelCreateRoom");
        }

        public void OnClickSwitchToLogin()
        {
            switchToPanel("panelLogin");
        }

        public void OnClickSwitchToLobby()
        {
            switchToPanel("panelLobby");
        }

        public void switchToPanel(string name)
        {
            panelCreateRoom.SetActive(name.Equals(panelCreateRoom.name));
            panelLobby.SetActive(name.Equals(panelLobby.name));
            panelLogin.SetActive(name.Equals(panelLogin.name));
        }

        // just for testing
        private void simulateRooms()
        {
            GameObject entry = Instantiate(objRoomListEntryPrefab);
            entry.transform.SetParent(objRoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            // We have to set z-coordinate explicitly to zero because it somehow has a random value after creation
            // and as a result the component is not visible (because of 2D view)
            entry.transform.localPosition = new Vector3(entry.transform.position.x, entry.transform.position.y, 0f);
            entry.GetComponent<RoomListEntry>().Initialize("test", (byte)5, 20, false);
        }
    }
}
