using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
        PlayerPrefs.SetInt("level", 1);
    }

    public void PlayBTN()
    {
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("level"));
        SceneManager.LoadScene("Introduction");
    }

    public void QuitBTN()
    {
        Application.Quit();
    }
}
