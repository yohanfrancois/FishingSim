using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform arrowDir;
    [SerializeField] private float arrowRotateSpeed;
    [SerializeField] private float aimAngleMin;
    [SerializeField] private float aimAngleMax;

    // Update is called once per frame
    void Update()
    {
        float currentZRotation = arrowDir.localEulerAngles.z;
        if (currentZRotation > 180f)
            currentZRotation -= 360f;

        Debug.Log(currentZRotation);
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
