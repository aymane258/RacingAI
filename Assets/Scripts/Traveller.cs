using UnityEngine;
using Random = UnityEngine.Random;

public class Traveller : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private Rigidbody Rigidbody;


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = Vector3.back * Random.Range(minSpeed,maxSpeed);
    }
}


