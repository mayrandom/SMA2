using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitializeScene : MonoBehaviour
{
    [SerializeField]
    GameObject SliderMemory;
    [SerializeField]
    GameObject InputTrust;
    [SerializeField]
    GameObject SliderCourage;

    public static float nbMemory;
    public static float nbTrust;
    public static float nbCourage;
    public static float startNbMemory;
    public static float startNbCourage;
    private string trust;

    public void Initialize()
    {
        nbMemory = SliderMemory.GetComponent<Slider>().value;
        trust = InputTrust.GetComponent<InputField>().text;
        nbTrust = float.Parse(trust);
        if (nbTrust > 100)
            nbTrust = 100;
        if (nbTrust < 0)
            nbTrust = 0;
        nbCourage = SliderCourage.GetComponent<Slider>().value;
        startNbMemory = nbMemory;
        startNbCourage = nbCourage;

        SceneManager.LoadScene("MainScene");
    }
}
