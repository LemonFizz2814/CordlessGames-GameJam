using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroductionScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("level"));
    }
}
