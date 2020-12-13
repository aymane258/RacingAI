using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Car : Agent
{
    public float force = 15f;
    private Rigidbody rigidbody = null;

    public override void Initialize()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] == 1)
        {
            Thrust();

        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            actionsOut[0] = 1;
            AddReward(-0.001f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Checkpoint")
        {
            AddReward(1.0f);
        }
        else if (collision.collider.tag == "Wall")
        {
            AddReward(-0.1f);
        }
    }

    private void Thrust()
    {
        rigidbody.AddForce(Vector3.up * force, ForceMode.Acceleration);
    }


}
