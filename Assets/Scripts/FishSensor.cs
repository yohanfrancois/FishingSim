using UnityEngine;

public class FishSensor : MonoBehaviour
{
    [SerializeField] private FishController fish = null;
    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("FishBait"))
		{
			FishBait fishBait = other.GetComponent<FishBait>();
			if (fishBait && !fishBait.HasFish)
				fish.DetectFishBait(fishBait);
		}
    }
	private void OnTriggerExit2D(Collider2D collision)
	{
		
	}
}
