using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.XR;

public class SteeringControl : MonoBehaviour
{
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
    private Rigidbody kartRB;

    public float currentWheelRotation = 0;

    private float turnDampening = 8000;
    public Transform directionalObject;

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

        kartRB = kart.GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandsRelease();
        HandrotationToSteerrotation();
        TurnKart();
        currentWheelRotation = -transform.rotation.eulerAngles.y;
    }

    private void TurnKart()
    {
        var turn = -transform.rotation.eulerAngles.y;
        if(turn < -350)
        {
            turn += 360; 
        }

        kartRB.MoveRotation(Quaternion.RotateTowards(kart.transform.rotation, Quaternion.Euler(0, turn, 0), Time.deltaTime * turnDampening));
    }

    private void HandsRelease()
    {
        if (rHandOnWheel && rightController.TryGetFeatureValue(CommonUsages.grip, out float rtriggerValue) && rtriggerValue <= 0)
        {
            rHand.transform.parent = rHandParent;
            rHand.transform.position = rHandParent.position;
            rHand.transform.rotation = rHandParent.rotation;
            rHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            rHandOnWheel = false;
        }
        if (lHandOnWheel && leftController.TryGetFeatureValue(CommonUsages.grip, out float ltriggerValue) && ltriggerValue <= 0)
        {
            lHand.transform.parent = lHandParent;
            lHand.transform.position = lHandParent.position;
            lHand.transform.rotation = lHandParent.rotation;
            lHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            lHandOnWheel = false;
        }
        /*if (!(lHandOnWheel && rHandOnWheel))
        {
            transform.parent = null;
        }*/
    }

    private void HandrotationToSteerrotation()
    {
        if (rHandOnWheel && !lHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, rHandParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        } else if (!rHandOnWheel && lHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, lHandParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        } else if (rHandOnWheel && lHandOnWheel)
        {
            Quaternion newRotR = Quaternion.Euler(0, 0, rHandParent.transform.rotation.eulerAngles.z);
            Quaternion newRotL = Quaternion.Euler(0, 0, lHandParent.transform.rotation.eulerAngles.z);
            Quaternion finalRot = Quaternion.Slerp(newRotL, newRotR, 1.0f / 2.0f);
            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
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
            if(point.childCount == 0)
            {
                var distance = Vector3.Distance(point.position, hand.transform.position);
                if (distance < shortestdistance)
                {
                    shortestdistance = distance;
                    closestPoint = point;
                }
            }
        }
        handParent = hand.transform.parent;

        hand.transform.parent = closestPoint.transform;
        hand.transform.position = closestPoint.transform.position;
        hand.transform.localScale = new Vector3(0.24f, 0.24f, 4.2f);

        handOnWheel = true;
        
    }
}
