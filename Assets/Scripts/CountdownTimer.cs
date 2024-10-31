using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 120f;
    [SerializeField] private TextMeshProUGUI countdownText;
    bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
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
        else if (timeRemaining <= 0)
        {
            timerIsRunning = false;
            countdownText.text = "00:00";
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
}
