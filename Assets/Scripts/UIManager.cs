using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayScreen;
    public GameObject gameoverScreen;
    public GameObject winScreen;

    public TextMeshProUGUI infoText;

    public Image pickUpImage;

    public Sprite[] pickUpSprites;

    void Start()
    {
        gameplayScreen.SetActive(true);
        gameoverScreen.SetActive(false);
        winScreen.SetActive(false);

        PickedUpImage(0);
    }

    public void PickedUpImage(int _i)
    {
        pickUpImage.sprite = pickUpSprites[_i];
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
}
