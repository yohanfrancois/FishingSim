using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float timerDuration = 120f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject inGamePanel;
    private float timeRemaining;
    bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = timerDuration;
        endGamePanel.SetActive(false);
        inGamePanel.SetActive(true);
        UpdateTimerDisplay();
        StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else if (timeRemaining <= 0 && timerIsRunning)
        {
            EndGame();
        }
    }

    public void StartCountdown()
    {
        timerIsRunning = true;
    }
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame()
    {
        timerIsRunning = false;
        timeRemaining = 0;
        countdownText.text = "00:00";
        endGamePanel.SetActive(true);
        inGamePanel.SetActive(false);
    }

    public void ResetTimer()
    {
        timeRemaining = timerDuration;
        timerIsRunning = false;
        UpdateTimerDisplay();
        endGamePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public float getTimeRemaining()
    {
        return timeRemaining;
    }
}
