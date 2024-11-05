using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartSceneButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private Button endGameSelected;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButton);
        restartSceneButton.onClick.AddListener(RestartSceneButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(endGameSelected.gameObject);
    }

    private void ResumeButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void RestartSceneButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
