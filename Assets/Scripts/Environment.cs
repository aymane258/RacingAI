using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Traveller travellerPrefab;

    private Thief thief;
    private TextMeshPro scoreBoard;
    private GameObject travellers;
    public float SpawnTime;
    public float delayInBetween;


    void Start()
    {
        InvokeRepeating("SpawnTravellers", SpawnTime, delayInBetween);
    }

    public void OnEnable()
    {
        travellers = transform.Find("Travellers").gameObject;
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
        thief = transform.GetComponentInChildren<Thief>();        
    }

    private void FixedUpdate()
    {
        scoreBoard.text = thief.GetCumulativeReward().ToString("f2");
    }
 


    public void ClearEnvironment()
    {
        travellers = transform.Find("Travellers").gameObject;

        foreach (Transform traveller in travellers.transform)
        {
            GameObject.Destroy(traveller.gameObject);
        }
    }


    public void SpawnTravellers()
    {
        GameObject newTraveller = Instantiate(travellerPrefab.gameObject);

        newTraveller.transform.SetParent(travellers.transform);
        newTraveller.transform.localPosition = travellers.transform.localPosition;
        newTraveller.transform.localRotation = travellers.transform.localRotation;

    }
}
