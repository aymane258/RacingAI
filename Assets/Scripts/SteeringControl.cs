using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class SteeringControl : MonoBehaviour
{
    public TextMeshPro text;

    InputDevice leftController;
    InputDevice rightController;

    public GameObject rHand;
    private Transform rHandParent;
    private bool rHandOnWheel = false;

    public GameObject lHand;
    private Transform lHandParent;
    private bool lHandOnWheel = false;

    public Transform[] snappoints;

    public GameObject kart;
    private newMovement _movement;
    private float rotation;

    private void Awake()
    {
        _movement = kart.GetComponent<newMovement>();
    }

    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics rightCh = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(rightCh, inputDevices);
        rightController = inputDevices[0];

        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        InputDeviceCharacteristics leftCh = InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(leftCh, inputDevices);
        leftController = inputDevices[0];
    }

    void FixedUpdate()
    {
        HandsRelease();
        HandrotationToSteerrotation();
    }

    private void HandrotationToSteerrotation()
    {
        float currentRotation = rotation;

        if (rHandOnWheel)
        {
            // Calcute angle
            Vector3 targetDir = rHand.transform.position - transform.position;
            float a = (Vector3.Angle(transform.forward, targetDir) - 180) / 180;
            currentRotation += a;
        }
        else if (lHandOnWheel)
        {
            // Calcute angle
            Vector3 targetDir = lHand.transform.position - transform.position;
            float a = (Vector3.Angle(transform.forward, targetDir) - 180) / 180;
            currentRotation += a;
        }else
        {
            currentRotation = 0;
        }

        text.text = $"Cur: {(currentRotation * 100)}";

        currentRotation = Mathf.Clamp(currentRotation, -1f, 1f);

        _movement.Steer(currentRotation);
        rotation = currentRotation;

        transform.rotation = Quaternion.Euler(0, 0, 180 * currentRotation);
    }

    private void HandsRelease()
    {
        if (rHandOnWheel && rightController.TryGetFeatureValue(CommonUsages.grip, out float rtriggerValue) && rtriggerValue <= 0)
        {
            rHand.SetActive(true);
            if(rHandParent.childCount > 0)
            {
                for (int i = 0; i < rHandParent.childCount; i++)
                {
                    rHandParent.GetChild(i).gameObject.SetActive(false);
                }
            }
            rHandOnWheel = false;
        }
        if (lHandOnWheel && leftController.TryGetFeatureValue(CommonUsages.grip, out float ltriggerValue) && ltriggerValue <= 0)
        {
            lHand.SetActive(true);
            if (lHandParent.childCount > 0)
            {
                for (int i = 0; i < lHandParent.childCount; i++)
                {
                    lHandParent.GetChild(i).gameObject.SetActive(false);
                }
            }
            lHandOnWheel = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Playerhand"))
        {
            if(!rHandOnWheel && rightController.TryGetFeatureValue(CommonUsages.grip, out float rtriggerValue) && rtriggerValue > 0)
            {
                PlaceHandOnWheel(ref rHand, ref rHandParent, ref rHandOnWheel);
            }
            if (!lHandOnWheel && leftController.TryGetFeatureValue(CommonUsages.grip, out float ltriggerValue) && ltriggerValue > 0)
            {
                PlaceHandOnWheel(ref lHand, ref lHandParent, ref lHandOnWheel);
            }
        }
    }

    private void PlaceHandOnWheel(ref GameObject hand, ref Transform handParent, ref bool handOnWheel)
    {
        var shortestdistance = Vector3.Distance(snappoints[0].position, hand.transform.position);
        var closestPoint = snappoints[0];

        foreach (var point in snappoints)
        {
                var distance = Vector3.Distance(point.position, hand.transform.position);
                if (distance < shortestdistance)
                {
                    shortestdistance = distance;
                    closestPoint = point;
                }
        }
        if(closestPoint.childCount > 0)
        {
            for (int i = 0; i < closestPoint.childCount; i++)
            {
                closestPoint.GetChild(i).gameObject.SetActive(true);
            }
        }
        hand.gameObject.SetActive(false);
        handParent = closestPoint;
        handOnWheel = true;
    }
}
