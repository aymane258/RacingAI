using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringControl : MonoBehaviour
{
    public GameObject rHand;
    private Transform rHandParent;
    private bool rHandOnWheel = false;

    public GameObject lHand;
    private Transform lHandParent;
    private bool lHandOnWheel = false;

    public Transform[] snappoints;

    public GameObject kart;
    public Rigidbody kartRB;

    public float currentWheelRotation = 0;

    private float turnDampening = 5000;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
