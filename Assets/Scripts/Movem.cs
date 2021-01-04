using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Movem : MonoBehaviour
{
    private InputDevice inputSource;
    private List<InputDevice> devices = new List<InputDevice>();
    Rigidbody rbKart;
    void Start()
    {
        InputDevices.GetDevices(devices);
        InputDeviceCharacteristics rightCh = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(rightCh, devices);
        inputSource = devices.FirstOrDefault();

        rbKart = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (inputSource.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed) && buttonPressed)
        {
            rbKart.velocity = transform.forward * 100.0f;
        }
    }
}
