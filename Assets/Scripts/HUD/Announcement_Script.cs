using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Announcement_Script : MonoBehaviour
    {
        private bool announcement_toggle = false;
        public Text shown_announcement;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                announcement_toggle = !announcement_toggle;
            }

            if (announcement_toggle == true)
            {
                shown_announcement.text = "Meldung!";
                shown_announcement.color = Color.red;
            }
            else 
            {
                shown_announcement.text = "Mit M melden";
                shown_announcement.color = new Color32(229,229,229,229);
            }
        }
    }
}
