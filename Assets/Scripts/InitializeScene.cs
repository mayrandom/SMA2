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
    private string trust;

    public void Initialize()
    {
        nbMemory = SliderMemory.GetComponent<Slider>().value;
        trust = InputTrust.GetComponent<InputField>().text;
        nbTrust = float.Parse(trust);
        nbCourage = SliderCourage.GetComponent<Slider>().value;

        SceneManager.LoadScene("MainScene");
    }
}
