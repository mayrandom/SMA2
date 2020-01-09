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
    GameObject SliderTrust;
    [SerializeField]
    GameObject SliderCourage;

    public static float nbMemory;
    public static float nbTrust;
    public static float nbCourage;

    public void Initialize()
    {
        nbMemory = SliderMemory.GetComponent<Slider>().value;
        nbTrust = SliderTrust.GetComponent<Slider>().value;
        nbCourage = SliderCourage.GetComponent<Slider>().value;
        SceneManager.LoadScene("MainScene");
    }
}
