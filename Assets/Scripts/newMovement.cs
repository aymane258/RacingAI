using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class newMovement : MonoBehaviour
{
    private Rigidbody body;
    public float maxSpeed = 30f;
    [Range(-1f, 1f)]
    public float rotation;
    public float rotationSpeed;
    public bool isRiding;
    [SerializeField]
    [Range(-1f,1f)]
    private float currentSpeed;
    public LayerMask layerMask;
    public float raydistance = 1f;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        currentSpeed = Mathf.Clamp(currentSpeed, -1f, 1f);
        rotation = Mathf.Clamp(rotation, -1f, 1f);
        float wheelSpeed = maxSpeed * currentSpeed;

        if (isRiding)
        {
            if(Physics.Raycast(transform.position, Vector3.down, raydistance, layerMask))
            {
                Debug.DrawRay(transform.position, Vector3.down * .1f, Color.green, raydistance);
                transform.Rotate(0, (rotation * currentSpeed) * rotationSpeed * Time.fixedDeltaTime, 0);
                Vector3 localVelocity = transform.rotation * (Vector3.forward * wheelSpeed);
                body.velocity = localVelocity;
            }
            
        }
    }

    internal void ApplyAcceleration(float v)
    {
        currentSpeed += v * .1f;
    }

    private void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime);
#if UNITY_EDITOR
        rotation = Mathf.Lerp(rotation, 0, Time.deltaTime);
        float LR = Input.GetAxis("Horizontal");
        float FB = Input.GetAxis("Vertical");

        currentSpeed += FB * .1f;
        rotation += LR * .1f;
#endif 
    }

    internal void Steer(float newRot)
    {
        rotation = newRot;
    }
}
