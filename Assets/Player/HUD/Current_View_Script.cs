using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Current_View_Script : MonoBehaviour
    {
        public Text shown_view;
        private string view;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            view = "TEXT";
            shown_view.text = view;
        }
    }
}
