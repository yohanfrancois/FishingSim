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
    private static bool isFishAttracted = false;
    private PlayerController playerController;
    private BoxCollider2D boxCollider;
    private GameObject fishBait;
    private float baitDetectionRadius;

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

        baitDetectionRadius = playerController.getBaitDetectionRadius();
    }

    private void Update()
    {
        if (isChasingBait && fishBait != null) 
        {
            float distanceToBait = Vector3.Distance(transform.position, fishBait.transform.position);

            if (distanceToBait <= baitDetectionRadius)
            {
                // Changer la couleur en rouge
                spriteRenderer.color = attractedColor;
            }
            else
            {
                // Revenir à la couleur normale
                spriteRenderer.color = normalColor;
            }

            // Le poisson suit l'appat si celui-ci est à portée
            direction = (fishBait.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = normalColor;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FishBait") && !isFishAttracted && other.transform.localPosition.y < -0.5f)
        {
            Debug.Log("Fish attracted");
            isFishAttracted = true; // Indique qu'un poisson est attiré
            fishBait = other.gameObject; // Référence à l'appât
            speed = targetedSpeed; // Augmenter la vitesse
            isChasingBait = true; // Commencer à suivre l'appât
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
        isFishAttracted = true; // Indique qu'un poisson est attiré
        fishBait = bait.gameObject; // Référence à l'appât
        speed = targetedSpeed; // Augmenter la vitesse
        isChasingBait = true; // Commencer à suivre l'appât
    }
}
