using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartSceneButton;
    [SerializeField] private Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButton);
        restartSceneButton.onClick.AddListener(RestartSceneButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    private void ResumeButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void RestartSceneButton()
    {
        SceneManager.LoadScene("Game");
    }

    private void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
