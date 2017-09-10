using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    //This script activates a 24 hour timer on first call and will then carry out actions when the current time is greater than the expiry time

    DateTime expiryTime;
    bool menuVisible = false;
    int firstRun = 0;

    public Text timerText;
    public GameObject pauseMenu;
    public GameObject creditMenu;
    public static bool timerFinished = false;
    public GameObject loadLevelButton;

    void Start()
    {
        if (!this.ReadTimestamp("timer")) //If timer hasn't been set yet, set it
        {
            this.ScheduleTimer();
        }
    }

    public void ResetTimer() //To reset the timer (attached to button)
    {
        PlayerPrefs.DeleteKey("timer");
        PlayerPrefs.SetInt("savedFirstRun", 0);
    }

    void LoadMainGame()
    {
        SceneManager.LoadScene("Dev0");
    }

    void Update()
    {
        firstRun = PlayerPrefs.GetInt("savedFirstRun");

        timerText.text = expiryTime.ToString();

        if (DateTime.Now > expiryTime)
        {
            timerFinished = true;
            PlayerPrefs.SetInt("savedFirstRun", 0);
            this.ScheduleTimer();
        }

        if (firstRun == 0)
        {
            loadLevelButton.SetActive(true);
            timerText.gameObject.SetActive(false);
            timerFinished = true;
        }
        else
        {
            loadLevelButton.SetActive(false);
            timerText.gameObject.SetActive(true);
        }

        Debug.Log(PlayerPrefs.GetInt("savedFirstRun"));
    }
    public void ScheduleTimer()
    {
        expiryTime = DateTime.Now.AddDays(1.0);
        this.WriteTimestamp("timer");
    }
    private bool ReadTimestamp(string key)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            return false;
        }
        expiryTime = DateTime.FromBinary(tmp);
        return true;
    }

    private void WriteTimestamp(string key)
    {
        PlayerPrefs.SetString(key, expiryTime.ToBinary().ToString());
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());
    }

    public void ShowMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadCreditMenu()
    {
        pauseMenu.SetActive(false);
        creditMenu.SetActive(true);
    }

    public void BacktoMain()
    {
        creditMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}