using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject keyboardControlsUI;
    [SerializeField] private GameObject controllerControlsUI;
    [SerializeField] private GameObject endGameUI;

    private bool isControllerConnected;
    private bool isDisabled = false;

    private void Start()
    {
        UpdateControlUI();
    }

    private void Update()
    {
        // Vérifie si l'appareil d'entrée est un clavier ou une manette
        if (Gamepad.current != null && !isControllerConnected)
        {
            isControllerConnected = true;
            UpdateControlUI();
        }
        else if (Gamepad.current == null && isControllerConnected)
        {
            isControllerConnected = false;
            UpdateControlUI();
        }
        else if (isDisabled)
        {
            DisableAll();
        }
    }

    private void UpdateControlUI()
    {
        // Affiche les icônes correspondantes selon le périphérique d'entrée
        if (isControllerConnected)
        {
            keyboardControlsUI.SetActive(false);
            controllerControlsUI.SetActive(true);
        }
        else
        {
            keyboardControlsUI.SetActive(true);
            controllerControlsUI.SetActive(false);
        }
    }

    private void DisableAll()
    {
        keyboardControlsUI.SetActive(false);
        controllerControlsUI.SetActive(false);
        endGameUI.SetActive(true);
    }

    public void setDisabled(bool value)
    {
        isDisabled = value;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
