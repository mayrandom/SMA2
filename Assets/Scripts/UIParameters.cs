using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIParameters : MonoBehaviour
{
    GameObject timeText;
    string convertedTime;
    private float minutes;
    private float seconds;
    private float timeNow;
    private GameObject[] agents;
    public static int nbCulprit;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GameObject.Find("TimeText");
        Time.timeScale = 1;
        timeNow = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeNow = Mathf.Round(Time.timeSinceLevelLoad);
        minutes = Mathf.Floor(timeNow / 60);
        seconds = Mathf.Round(timeNow % 60);

        if (seconds == 60)
        {
            seconds = 0;
            minutes++;
        }
        timeText.GetComponent<Text>().text = "Temps écoulé \n" + minutes + " : " + seconds;

        if (minutes >= 5)
        {
            agents = GameObject.FindGameObjectsWithTag("agent");
            for (int i = 0; i < agents.Length; i++)
            {
                if (agents[i].GetComponent<AgentController>().culpritFound)
                    nbCulprit++;
            }
            SceneManager.LoadScene("EndScene");
        }
            
    }
}
