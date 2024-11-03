using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private int nbFishToGenerate = 5;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private FishBait fishBait = null;
    [SerializeField] private PlayerController player = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nbFishToGenerate; i++)
        {
            generateFish();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateFish()
    {
        GameObject fish = Instantiate(fishPrefab, transform);
        fish.transform.localPosition = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 0.0f), 0);
        float scale = Random.Range(minScale, maxScale);
        //fish.transform.localScale = new Vector3(scale, scale / 3, 1);
        fish.GetComponent<FishController>().setFishBait(fishBait.gameObject);
        fish.GetComponent<FishController>().setPlayerController(player);
    }
}
