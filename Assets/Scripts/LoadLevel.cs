using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used at the loading lobby -- not nessecary if building straight to main game

public class LoadLevel : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            LoadMainLevel();
        }
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene("Dev0");
    }
}