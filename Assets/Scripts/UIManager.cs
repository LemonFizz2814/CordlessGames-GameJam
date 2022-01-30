using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayScreen;
    public GameObject gameoverScreen;
    public GameObject winScreen;
    public GameObject heartObj;
    public GameObject ammoObj;

    public TextMeshProUGUI infoText;

    public Image pickUpImage;

    public Slider barSlider;

    public Sprite[] pickUpSprites;
    public Sprite[] heartSprites;
    public Sprite[] ammoSprites;

    int totalLevels = 3;

    void Start()
    {
        barSlider.value = 1;

        gameplayScreen.SetActive(true);
        gameoverScreen.SetActive(false);
        winScreen.SetActive(false);

        PickedUpImage(0);
    }

    public void PickedUpImage(int _i)
    {
        pickUpImage.sprite = pickUpSprites[_i];
    }

    public void UpdateHeartImages(int _health)
    {
        for(int i = 0; i < heartObj.transform.childCount; i++)
        {
            heartObj.transform.GetChild(i).GetComponent<Image>().sprite = (i >= _health) ? heartSprites[0] : heartSprites[1];
        }
    }
    public void UpdateAmmoImages(int _ammo)
    {
        for(int i = 0; i < ammoObj.transform.childCount; i++)
        {
            ammoObj.transform.GetChild(i).GetComponent<Image>().sprite = (i >= _ammo) ? ammoSprites[0] : ammoSprites[1];
        }
    }

    public void ChangeBar(int _i)
    {
        barSlider.value = _i;
    }

    public void ShowWinScreen()
    {
        gameplayScreen.SetActive(false);
        winScreen.SetActive(true);
    }
    public void ShowGameOverScreen()
    {
        gameplayScreen.SetActive(false);
        gameoverScreen.SetActive(true);
    }

    public void TextAnimation(string _string)
    {
        infoText.GetComponent<Animation>().Play();
        infoText.text = _string;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void NextLevel()
    {
        if ((PlayerPrefs.GetInt("level") + 1) <= totalLevels)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("level"));
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("level"));
    }
}
