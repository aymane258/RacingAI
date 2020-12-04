using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class Thief : Agent
{

    [SerializeField] private float jumpForce;
    [SerializeField] private KeyCode jumpKey;
    private Vector3 startingPosition;
    private bool jumpIsReady = true;
    private bool stoleMoney = false;
    private bool firstCollision = true;

    private Rigidbody body;
    private Environment environment;

    public event Action OnReset;


    public override void Initialize()
    {
        body = GetComponent<Rigidbody>();
        environment = GetComponentInParent<Environment>();
        startingPosition = transform.position;

    }
 
    public override void OnEpisodeBegin()
    {
        jumpIsReady = true;
        transform.position = startingPosition;
        body.velocity = Vector3.zero;

        environment.ClearEnvironment();
        OnReset?.Invoke();
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if (Input.GetKey(jumpKey))
            actionsOut[0] = 1;
    }

    private void Jump()
    {
        if (jumpIsReady)
        {
            body.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            jumpIsReady = false;
        }

 
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
            Jump();

    }

    private void OnCollisionEnter(Collision collision)
    {
    if(collision.gameObject.CompareTag("Street") && firstCollision)
        {
            jumpIsReady = true;
            firstCollision = false;
        }

       else if (collision.gameObject.CompareTag("Street") && !stoleMoney) { 
                 AddReward(-0.2f);
                jumpIsReady = true;

        }
        else if (collision.gameObject.CompareTag("Street") && stoleMoney)
        {
            jumpIsReady = true;
            stoleMoney = false;
        }
        else if (collision.gameObject.CompareTag("Traveller"))
        {
                AddReward(-1.0f);
                EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Point"))
        {
            AddReward(0.1f);
            stoleMoney = true;
        }
    }
}

