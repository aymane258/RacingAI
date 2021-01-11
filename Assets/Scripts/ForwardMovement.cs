using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class ForwardMovement : MonoBehaviour
{
    private InputDevice inputSource;
    private newMovement _movement;
    private List<InputDevice> devices = new List<InputDevice>();
    Rigidbody rbKart;

    void Start()
    {
        _movement = GetComponent<newMovement>();

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
            _movement.ApplyAcceleration(1f);
        }
        if (inputSource.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonPressed2) && buttonPressed2)
        {
            _movement.ApplyAcceleration(-1f);
        }
    }
}
