using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{
    public class Show_Vote_Script : MonoBehaviour
    {
        public Text voteTopic;
    // Start is called before the first frame update
    void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetText(string x)
        {
            voteTopic.text = x;
        }
    }
}
