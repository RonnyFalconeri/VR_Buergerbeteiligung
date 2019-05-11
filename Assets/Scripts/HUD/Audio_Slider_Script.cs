using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRRoom
{

    public class Audio_Slider_Script : MonoBehaviour
    {
        public Slider volumeSlider;
        public AudioSource musicAudio;

        // Start is called before the first frame update
        void Start()
        {
            Transform player = GameObject.Find("OVRPlayerController").transform;
            musicAudio = (AudioSource)player.GetComponent("AudioSource");
        }

        public void VolumeController()
        {
            // Gets called for every slider movement
            musicAudio.volume = volumeSlider.value;
        }
    }
}
