using UnityEngine;

public class FishSensor : MonoBehaviour
{
    [SerializeField] private FishController fish = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishBait"))
        {
            FishBait fishBait = other.GetComponent<FishBait>();
            if (fishBait)
            fish.DetectFishBait(fishBait);
        }
    }
}
