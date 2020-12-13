using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


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
}
