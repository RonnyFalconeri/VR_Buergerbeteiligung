using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class VR_Configuration : MonoBehaviour
{
    public GameObject hudvr;
    public GameObject hudmouse;
    public GameObject gameMenuvr;
    public GameObject gameMenumouse;
    public GameObject modVoteMenuvr;
    public GameObject modVoteMenumouse;
    public GameObject voteMenuvr;
    public GameObject voteMenumouse;

    public GameObject eventSystem;
    public GameObject vrHands;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("VR enabled: " + XRDevice.isPresent);
        XRSettings.enabled = XRDevice.isPresent;

        if (false == XRDevice.isPresent)
        {
            // Enable cursor if no vr device is connected
            // Otherwise the user couldn't click on our fancy HUD menu :)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.Log("deactivate mosue");
            Transform player = GameObject.Find("OVRPlayerController").transform;
            OVRPlayerController controller = (OVRPlayerController)player.GetComponent("OVRPlayerController");
            controller.SetSkipMouseRotation(true);
        }

        // disable hands if no VR device connected (else we get spammed with error messages)
        vrHands.SetActive(XRDevice.isPresent);
        if ( false == XRDevice.isPresent )
        {
            GameObject avatarMng = GameObject.Find("OvrAvatarSDKManager");
            if (null != avatarMng)
            {
                avatarMng.SetActive(false);
            }
        }

        configureVRControlability();
    }

    public void configureVRControlability()
    {
        bool enableVRcontrol = XRDevice.isPresent;

        // configure whether elements shall be clicked by vr controlers or mouse
        hudvr.SetActive(enableVRcontrol);
        hudmouse.SetActive(!enableVRcontrol);
        gameMenuvr.SetActive(enableVRcontrol);
        gameMenumouse.SetActive(!enableVRcontrol);
        modVoteMenuvr.SetActive(enableVRcontrol);
        modVoteMenumouse.SetActive(!enableVRcontrol);
        voteMenuvr.SetActive(enableVRcontrol);
        voteMenumouse.SetActive(!enableVRcontrol);

        StandaloneInputModule mouseInput = (StandaloneInputModule)eventSystem.GetComponent("StandaloneInputModule");
        if ( null != mouseInput )
        {
            mouseInput.enabled = !enableVRcontrol;
        }
        ControllerSelection.OVRInputModule vrInput = (ControllerSelection.OVRInputModule)eventSystem.GetComponent("OVRInputModule");
        if (null != vrInput)
        {
            vrInput.enabled = enableVRcontrol;
        }
    }
}
