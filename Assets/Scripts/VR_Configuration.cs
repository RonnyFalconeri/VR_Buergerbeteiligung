using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class VR_Configuration : MonoBehaviour
{
    public GameObject hud;
    public GameObject gameMenu;
    public GameObject modVoteMenu;
    public GameObject voteMenu;

    public GameObject eventSystem;
    public GameObject vrHands;

    private GameObject[] controlables;

    // Start is called before the first frame update
    void Start()
    {
        controlables = new GameObject[4];
        controlables[0] = hud;
        controlables[1] = gameMenu;
        controlables[2] = modVoteMenu;
        controlables[3] = eventSystem;

        Debug.Log("VR enabled: " + XRDevice.isPresent);
        XRSettings.enabled = XRDevice.isPresent;

        if (false == XRDevice.isPresent)
        {
            // Enable cursor if no vr device is connected
            // Otherwise the user couldn't click on our fancy HUD menu :)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // disable hands if no VR device connected (else we get spammed with error messages)
        vrHands.SetActive(XRDevice.isPresent);

        configureVRControlability();
    }

    public void configureVRControlability()
    {
        bool enableVRcontrol = XRDevice.isPresent;

        // configure whether elements shall be clicked by vr controlers or mouse
        foreach (GameObject controlable in controlables)
        {
            // I don't know why but this OVRRaycaster is never found...But it's no big problem.
            // We just leave it enabled, because it doesn't disturb mouse control (just the other way around)
            ControllerSelection.OVRRaycaster vrray = (ControllerSelection.OVRRaycaster)controlable.GetComponent("OVR Raycaster");
            if ( null != vrray )
            {
                vrray.enabled = enableVRcontrol;
            }
            GraphicRaycaster mouseRay = (GraphicRaycaster)controlable.GetComponent("GraphicRaycaster");
            if (null != mouseRay)
            {
                mouseRay.enabled = !enableVRcontrol;
            }
        }

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
