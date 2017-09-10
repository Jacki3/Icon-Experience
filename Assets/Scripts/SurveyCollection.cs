using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SurveyCollection : MonoBehaviour
{
    //Collects data using check boxes then writes the answers to file (can change questions within the editor (just change them below too!))

    public ToggleGroup somethingNew;
    public ToggleGroup haveFun;
    public ToggleGroup Engaging;
    public Text dataPathtext;

    string filePath;
    GameObject timeKeeperObj;

    void Start()
    {
        filePath = Application.dataPath + "/Icons" + "/SurveyAnswers.txt"; //Can be custom (goes to /assets folder)

        if (File.Exists(filePath))
        {
            PlayerPrefs.SetInt("savedFirstRun", 1);
            SceneManager.LoadScene("WaitScreen");
        }
    }

    public void WriteAnswersToFile()
    {
        Toggle somethingNewAnswer = somethingNew.ActiveToggles().FirstOrDefault();
        Toggle haveFunAnswer = haveFun.ActiveToggles().FirstOrDefault();
        Toggle EngagingAnswer = Engaging.ActiveToggles().FirstOrDefault();

        var sr = File.CreateText(filePath);
        sr.WriteLine("Did you learn something new?");
        sr.WriteLine(somethingNewAnswer.gameObject.name);
        sr.WriteLine("Was it fun?");
        sr.WriteLine(haveFunAnswer.gameObject.name);
        sr.WriteLine("Was the story engaging?");
        sr.WriteLine(EngagingAnswer.gameObject.name);
        sr.Close();

        PlayerPrefs.SetInt("savedFirstRun", 1);
        SceneManager.LoadScene("WaitScreen");
    }


}