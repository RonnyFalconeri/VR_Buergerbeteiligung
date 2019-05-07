using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Vote_Script : MonoBehaviour
    {
        public InputField Field;
        public string TextFromInput;

        public void GetText()
        {
            TextFromInput = Field.text;
            Debug.Log(TextFromInput);
        }
    }
}
