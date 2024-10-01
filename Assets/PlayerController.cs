using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform arrowDir;
    [SerializeField] private float arrowRotateSpeed;
    [SerializeField] private float aimAngleMin;
    [SerializeField] private float aimAngleMax;
    [SerializeField] private GameObject fishBait;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwChargeSpeed;
    [SerializeField] private GameObject throwChargeBarGO;
    [SerializeField] private Image throwChargeBar;

    private bool isChargingThrow = false;
    private bool isChargingUp = true;
    private float throwCharge = 0f;

    private void Start()
    {
        throwChargeBarGO.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        float currentZRotation = arrowDir.localEulerAngles.z;
        if (currentZRotation > 180f)
            currentZRotation -= 360f;

        if (Input.GetKey(KeyCode.A))
        {
            // Calculez la nouvelle rotation en ajoutant l'entrée utilisateur
            float newZRotation = currentZRotation - arrowRotateSpeed * Time.deltaTime;

            if (newZRotation < aimAngleMin)
                newZRotation = aimAngleMin;

            arrowDir.rotation = Quaternion.Euler(0f, 180f, newZRotation);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Calculez la nouvelle rotation en ajoutant l'entrée utilisateur
            float newZRotation = currentZRotation + arrowRotateSpeed * Time.deltaTime;

            if (newZRotation > aimAngleMax)
                newZRotation = aimAngleMax;

            arrowDir.rotation = Quaternion.Euler(0f, 180f, newZRotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowFishBait();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            throwChargeBarGO.SetActive(true);
            throwChargeBar.fillAmount = 0f;
            isChargingThrow = true;
            throwCharge = 0f;
            isChargingUp = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isChargingThrow = false;
            ThrowFishBait();
            throwChargeBarGO.SetActive(false);
            throwCharge = 0f;
        }
        if (isChargingThrow)
        {
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

    }

    private void ThrowFishBait()
    {
        fishBait.transform.localPosition = Vector3.zero;
        fishBait.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fishBait.GetComponent<Rigidbody2D>().AddForce(arrowDir.up * (throwForce * (throwCharge / 100f)) );
    }

    public void OnMoveArrow(InputAction.CallbackContext context)
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
