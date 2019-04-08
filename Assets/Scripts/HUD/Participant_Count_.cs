using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{
    public class Participant_Count_ : MonoBehaviour
    {
        private int participants = 0;
        public Text participant_count;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            participant_count.text = "Teilnehmer: " + participants;
            if (Input.GetKeyDown(KeyCode.Plus))
            {
                participants++;
            }
        }
    }

}