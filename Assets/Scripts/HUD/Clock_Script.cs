using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Clock_Script : MonoBehaviour
    {
        public Text clock;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            DateTime time = DateTime.Now;
            string hour_string = "" + time.Hour;
            string minute_string = "" + time.Minute;
            string second_string = "" + time.Second;
            if (time.Hour < 10)
            {
                hour_string = "0" + hour_string;
            }
            if (time.Minute < 10)
            {
                minute_string = "0" + minute_string;
            }
            if (time.Second < 10)
            {
                second_string = "0" + second_string;
            }
            clock.text = hour_string + ":" + minute_string + ":" + second_string;
        }
    }
}
