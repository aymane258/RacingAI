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
    private KartController _kartController;
    private float newRot;

    private void Awake()
    {
        _kartController = kart.GetComponent<KartController>();
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

        _kartController = kart.GetComponent<KartController>();
    }

    void Update()
    {
        HandsRelease();
        HandrotationToSteerrotation();
    }

    private void HandrotationToSteerrotation()
    {
        if (!rHandOnWheel && !lHandOnWheel)
        {
            
        }
        else if (rHandOnWheel)
        {
            //newRot = rHand.transform.rotation.eulerAngles.z;
            //newRot = Vector3.Dot(rHand.transform.parent.position, rHandParent.position);
            //rHandParent.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, (newRot/2)+1 );

            Vector3 targetDir = rHand.transform.position - transform.position;
            newRot = (Vector3.Angle(transform.forward, targetDir) - 180) / 180;
        }
        else if (lHandOnWheel)
        {
            //newRot = lHand.transform.rotation.eulerAngles.z;
            //newRot = Vector3.Dot(lHand.transform.parent.position, lHandParent.position);
            //lHandParent.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, (newRot / 2) + 1);

            Vector3 targetDir = lHand.transform.position - transform.position;
            newRot = (Vector3.Angle(transform.forward, targetDir) - 180) / 180;

            //newRot = (Vector3.Angle(rHandParent.GetChild(0).transform.position, lHand.transform.position) - 180) / 180;
        }
        _kartController.Steer(newRot);
    }

    private void HandsRelease()
    {
        if (rHandOnWheel && rightController.TryGetFeatureValue(CommonUsages.grip, out float rtriggerValue) && rtriggerValue <= 0)
        {
            //rHand.transform.parent = rHandParent;
            //rHand.transform.position = rHandParent.position;
            //rHand.transform.rotation = rHandParent.rotation;
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
            //lHand.transform.parent = lHandParent;
            //lHand.transform.position = lHandParent.position;
            //lHand.transform.rotation = lHandParent.rotation;
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

        //hand.transform.parent = closestPoint.transform;
        //hand.transform.position = closestPoint.transform.position;

        handOnWheel = true;
        
    }
}
