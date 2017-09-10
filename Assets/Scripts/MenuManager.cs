using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //Handles menu functions -- each method is attached to buttons on the menu (create new methods and UI to generate a more complex menu)
    public GameObject pauseMenu;
    public GameObject creditMenu;
    public GameObject iconSheet;
    public GameObject taskNotificationText;
    public GameObject startButton;
    public GameObject countDownTimer;
    public GameObject Playtimer;
    public GameObject ScoreText;

    GameObject taskListener;
    TaskListener taskListenerScript;

    public void Start()
    {
        taskListener = GameObject.Find("TaskListener");
        taskListenerScript = taskListener.GetComponent<TaskListener>();
    }

    public void ShowMenu()
    {
        pauseMenu.SetActive(true);
        iconSheet.SetActive(false);
        taskNotificationText.SetActive(true);
        countDownTimer.SetActive(false);
        Time.timeScale = 0;
    }

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
        if (taskListenerScript.timerOn == true)
        {
            iconSheet.SetActive(true);
            countDownTimer.SetActive(true);
            taskNotificationText.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void LoadCreditMenu()
    {
        pauseMenu.SetActive(false);
        creditMenu.SetActive(true);
        taskNotificationText.SetActive(false);
        startButton.SetActive(false);
        ScoreText.SetActive(false);
        Playtimer.SetActive(false);
    }

    public void BacktoMain()
    {
        creditMenu.SetActive(false);
        pauseMenu.SetActive(true);
        taskNotificationText.SetActive(true);
        ScoreText.SetActive(true);
        Playtimer.SetActive(true);
        if (taskListenerScript.timerOn == false)
        {
            startButton.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
