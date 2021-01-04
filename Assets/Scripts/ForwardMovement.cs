﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class ForwardMovement : MonoBehaviour
{
    private InputDevice inputSource;
    private KartController _kartController;
    private List<InputDevice> devices = new List<InputDevice>();
    Rigidbody rbKart;

    void Start()
    {
        _kartController = GetComponent<KartController>();

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
            _kartController.ApplyAcceleration(1f);
        }
    }
}
