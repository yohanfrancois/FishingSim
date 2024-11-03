using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Fishing Rod")]
    [SerializeField] private Transform arrowDir;
    [SerializeField] private float arrowRotateSpeed;
    [SerializeField] private float aimAngleMin;
    [SerializeField] private float aimAngleMax;
    [SerializeField] private GameObject fishBait;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwChargeSpeed;
    [SerializeField] private GameObject throwChargeBarGO;
    [SerializeField] private Image throwChargeBar;
    [SerializeField] private float baitDetectionRadius = 1.0f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI endGameScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Fish")]
    [SerializeField] private FishSpawner fishSpawner;
    [SerializeField] private float timeBeforeSpawningANewFish = 5.0f;

    [Header("Others")]
    [SerializeField] private ControlsUI controlsUI;
    [SerializeField] private float windingFishSpeed;
    [SerializeField] private float windingResistanceTime;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private HighScoreManager highScoreManager;

    private int score = 0;
    private bool isChargingThrow = false;
    private bool isChargingUp = true;
    private bool isLaunched = false;
    private bool isChargeCanceled = false;
    private bool isWindingFishingRod = false;
    private float throwCharge = 0f;
    private float windingResistanceTimer;

    private void Start()
    {
        throwChargeBarGO.SetActive(false);
        fishBait.SetActive(false);
        scoreText.text = string.Format("{0:000}", score);
    }


    // Update is called once per frame
    void Update()
    {
        if (countdownTimer.getTimeRemaining() > 0 || SceneManager.GetActiveScene().name == "Tutorial")
        {
            float currentZRotation = arrowDir.localEulerAngles.z;
            if (currentZRotation > 180f)
                currentZRotation -= 360f;

            if (Input.GetKey(KeyCode.A) && !isChargingThrow && !isLaunched)
            {
                // Calculez la nouvelle rotation en ajoutant l'entr�e utilisateur
                float newZRotation = currentZRotation - arrowRotateSpeed * Time.deltaTime;

                if (newZRotation < aimAngleMin)
                    newZRotation = aimAngleMin;

                arrowDir.rotation = Quaternion.Euler(0f, 180f, newZRotation);
            }
            if (Input.GetKey(KeyCode.D) && !isChargingThrow && !isLaunched)
            {
                // Calculez la nouvelle rotation en ajoutant l'entr�e utilisateur
                float newZRotation = currentZRotation + arrowRotateSpeed * Time.deltaTime;

                if (newZRotation > aimAngleMax)
                    newZRotation = aimAngleMax;

                arrowDir.rotation = Quaternion.Euler(0f, 180f, newZRotation);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isChargingThrow && !isLaunched)
            {
                throwChargeBarGO.SetActive(true);
                throwChargeBar.fillAmount = 0f;
                isChargingThrow = true;
                throwCharge = 0f;
                isChargingUp = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && isChargingThrow)
            {
                fishBait.SetActive(true);
                isChargingThrow = false;
                ThrowFishBait();
                throwChargeBarGO.SetActive(false);
                throwCharge = 0f;
            }
            if (isChargingThrow)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    isChargingThrow = false;
                    throwChargeBarGO.SetActive(false);
                    return;
                }
                if (isChargingUp)
                {
                    throwCharge += throwChargeSpeed * Time.deltaTime;
                    if (throwCharge > 100f)
                    {
                        throwCharge = 100f;
                        isChargingUp = false;
                    }
                }
                else
                {
                    throwCharge -= throwChargeSpeed * Time.deltaTime;
                    if (throwCharge < 0f)
                    {
                        throwCharge = 0f;
                        isChargingUp = true;
                    }
                }

                throwChargeBar.fillAmount = throwCharge / 100f;
            }
            if (Input.GetKeyDown(KeyCode.Q) && isLaunched)
            {
                FishController[] allFish = FindObjectsOfType<FishController>();

                foreach (FishController fish in allFish)
                {
                    float distanceToBait = Vector3.Distance(fish.transform.position, fishBait.transform.position);
                    // Debug.Log(fish.transform.position + ", " + fishBait.transform.position + ", " + distanceToBait);
                    if (distanceToBait <= baitDetectionRadius)
                    {
                        if (SceneManager.GetActiveScene().name == "Tutorial")
                        {
                            controlsUI.setDisabled(true);
                            Destroy(fish.gameObject);
                        }
                        else
                        {
                            FishController.setIsFishAttracted(false);
                            Destroy(fish.gameObject);
                            score += Mathf.FloorToInt(50 * fish.transform.localScale.x);
                            scoreText.text = string.Format("{0:000}", score);

                            StartCoroutine(RespawnFishAfterDelay());
                        }
                    }
                }

                fishBait.transform.localPosition = Vector3.zero;
                fishBait.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                fishBait.SetActive(false);
                isLaunched = false;
            }
        }
        else
        {
            endGameScoreText.text = string.Format("{0:000}", score);
            highScoreManager.SaveHighScore(score);
            highScoreText.text = string.Format("{0:000}", highScoreManager.GetHighScore());
            fishBait.GetComponent<LineRenderer>().gameObject.SetActive(false);
        }

    }

    private void ThrowFishBait()
    {
        fishBait.transform.localPosition = Vector3.zero;
        fishBait.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fishBait.GetComponent<Rigidbody2D>().AddForce(arrowDir.up * (throwForce * (throwCharge / 100f)) );
        isLaunched = true;

    }

    public void OnMoveArrow(InputAction.CallbackContext context)
    {
        if (!isChargingThrow && !isLaunched)
        {
            Vector2 input = context.ReadValue<Vector2>().normalized;
            float angle = Vector2.SignedAngle(input, Vector2.up);

            if (angle < aimAngleMin)
                angle = aimAngleMin;
            if (angle > aimAngleMax)
                angle = aimAngleMax;

            arrowDir.rotation = Quaternion.Euler(0f, 180f, angle);
        }
    }

    public void OnThrowFishBait(InputAction.CallbackContext context)
    {
        if (context.started && !isChargingThrow && !isLaunched)
        {
            fishBait.SetActive(false);
            throwChargeBarGO.SetActive(true);
            throwChargeBar.fillAmount = 0f;
            isChargingThrow = true;
            isChargeCanceled = false;
            throwCharge = 0f;
            isChargingUp = true;
        }
        if (context.canceled && isChargingThrow && !isLaunched && !isChargeCanceled)
        {
            fishBait.SetActive(true);
            isChargingThrow = false;
            ThrowFishBait();
            throwChargeBarGO.SetActive(false);
            throwCharge = 0f;
        }
    }

    public void OnCancelThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isChargingThrow = false;
            isChargeCanceled = true;
            throwChargeBarGO.SetActive(false);
            Debug.Log("Throw canceled");
        }
    }

    public void OnCallFishBait(InputAction.CallbackContext context)
    {
        if (context.started && isLaunched)
        {
            /*
            FishController[] allFish = FindObjectsOfType<FishController>();

            foreach (FishController fish in allFish)
            {
                float distanceToBait = Vector3.Distance(fish.transform.position, fishBait.transform.position);
                // Debug.Log(fish.transform.position + ", " + fishBait.transform.position + ", " + distanceToBait);
                if (distanceToBait <= baitDetectionRadius)
                {
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        controlsUI.setDisabled(true);
                        Destroy(fish.gameObject);
                    }
                    else
                    {
                        FishController.setIsFishAttracted(false);
                        Destroy(fish.gameObject);
                        score += Mathf.FloorToInt(50 * fish.transform.localScale.x);
                        scoreText.text = string.Format("{0:000}", score);

                        StartCoroutine(RespawnFishAfterDelay());
                    }
                }
            }

            fishBait.transform.localPosition = Vector3.zero;
            fishBait.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isLaunched = false;
            */
        }
    }

    private IEnumerator RespawnFishAfterDelay()
    {
        yield return new WaitForSeconds(timeBeforeSpawningANewFish);
        fishSpawner.generateFish();
    }

    public void OnWindingFishingRod(InputAction.CallbackContext context)
    {
        if (context.started && isLaunched)
        {
            isWindingFishingRod = true;
            /*
            FishController[] allFish = FindObjectsOfType<FishController>();

            foreach (FishController fish in allFish)
            {
                float distanceToBait = Vector3.Distance(fish.transform.position, fishBait.transform.position);
                Debug.Log(fish.transform.position + ", " + fishBait.transform.position + ", " + distanceToBait);
                if (distanceToBait <= baitDetectionRadius)
                {
                    Destroy(fish.gameObject);
                }
            }

            fishBait.transform.localPosition = Vector3.zero;
            fishBait.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isLaunched = false;
            */
        }
        if (context.canceled && isLaunched)
        {
            isWindingFishingRod = false;
        }
    }

    private void OnWindingFishingRod()
    {
        fishBait.GetComponent<Rigidbody2D>().velocity = (transform.position - fishBait.transform.position).normalized * Time.deltaTime * windingFishSpeed;
    }

    public float getBaitDetectionRadius()
    {
        return baitDetectionRadius;
    }

}
