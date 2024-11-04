using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float normalSpeed = 5.0f;
    [SerializeField] private float targetedSpeed = 7.0f;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color attractedColor = Color.red;

    [Header("Others")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private FishSensor fishSensor = null;

    
    private Vector3 direction;
    private float speed;
    private bool isChasingBait = false;
	private bool isCatched = false;
    private static bool isFishAttracted = false;
    private PlayerController playerController;
    private GameObject fishBait;

	// Getter/Setter
	public bool IsChasingBait => isChasingBait;

    private void Start()
    {
        int number = Random.Range(0, 2);
        if (number == 0)
        {
            direction = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            direction = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        speed = normalSpeed;
        if (!spriteRenderer)
            Debug.Log("What?");
        spriteRenderer.color = normalColor;
    }

    private void Update()
    {
        if (isChasingBait && fishBait != null) 
        {
            // Le poisson suit l'appat si celui-ci est à portée
            direction = (fishBait.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else if (!isCatched)
        {
            // Logique de mouvement normal lorsque le poisson ne suit pas l'appât
            if (transform.position.x > 8)
            {
                direction = Vector3.left;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (transform.position.x < -8)
            {
                direction = Vector3.right;
                transform.rotation = Quaternion.Euler(0, 0, 0);   
            }
			transform.position += direction * speed * Time.deltaTime;
        }
        // if is catched
        else if (fishBait.GetComponent<FishBait>().IsBeingPulled == false)
        {
            fishBait.GetComponent<Rigidbody2D>().velocity = direction * speed / 4f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FishBait") /*&& Vector3.Distance(transform.position, other.transform.position) > playerController.getBaitDetectionRadius()*/)
        {
            isFishAttracted = false; // Indique qu'aucun poisson n'est attiré
            speed = normalSpeed; // Réinitialiser la vitesse
            isChasingBait = false; // Arrêter de suivre l'appât
            Debug.Log("Fish not attracted, current direction" + direction);
            direction = new Vector3(direction.x, 0, 0).normalized; // Changer de direction
            Debug.Log("Fish not attracted, exited with direction" + direction);
            spriteRenderer.color = normalColor; // Réinitialiser la couleur
        }
    }

    public void setFishBait(GameObject fishBaitOther)
    {
        fishBait = fishBaitOther;
    }

    public void setPlayerController(PlayerController playerControllerOther)
    {
        playerController = playerControllerOther;
    }

    public static void setIsFishAttracted(bool isFishAttractedOther)
    {
        isFishAttracted = isFishAttractedOther;
    }

    public void DetectFishBait(FishBait bait)
    {
		Debug.Log("Has detected the bait.");
        isFishAttracted = true; // Indique qu'un poisson est attiré
        fishBait = bait.gameObject; // Référence à l'appât
        speed = targetedSpeed; // Augmenter la vitesse
        isChasingBait = true; // Commencer à suivre l'appât
    }

	internal void Catched()
	{
		isCatched = true;
		isChasingBait = false;
		spriteRenderer.color = attractedColor;
		transform.SetParent(fishBait.transform);
		transform.localPosition = Vector3.zero;
        direction = transform.position - playerController.transform.position;
        transform.right = direction;
	}
}
