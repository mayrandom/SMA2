using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParameters : MonoBehaviour
{
    GameObject timeText;
    string convertedTime;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GameObject.Find("TimeText");
        if (timeText == null)
            Debug.Log("caca");
    }

    // Update is called once per frame
    void Update()
    {
        float timeNow = Time.realtimeSinceStartup;
        float minutes = Mathf.Round(timeNow / 60);
        float seconds = Mathf.Round(timeNow % 60);
        if (timeText == null)
            Debug.Log("caca");
        timeText.GetComponent<Text>().text = "Temps écoulé \n" + minutes + " : " + seconds;

    }
}
