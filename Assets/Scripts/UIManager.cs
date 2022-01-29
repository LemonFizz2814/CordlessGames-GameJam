using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayScreen;
    public GameObject gameoverScreen;
    public GameObject winScreen;

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
}
