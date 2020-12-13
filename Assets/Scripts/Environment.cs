using TMPro;
using System.Collections;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private TextMeshPro scoreBoard;

    private Car car;

    public void OnEnable()
    {
        car = transform.GetComponentInChildren<Car>();
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
    }

    private void FixedUpdate()
    {
        scoreBoard.text = car.GetCumulativeReward().ToString("f2");
    }
}
