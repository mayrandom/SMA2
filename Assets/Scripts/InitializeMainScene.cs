using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMainScene : MonoBehaviour
{
    private string[] noms;
    private double[] trustValues;

    bool memory;
    double trust;
    bool courage;

    // Start is called before the first frame update
    void Start()
    {
        CreateAgents();
    }

    public void CreateAgents()
    {
        //Initialization of initial agents position
        Vector3 pos1 = new Vector3(-37.96f, 3.18f, -15.49f);
        Vector3 pos2 = new Vector3(13.49f, 7.1f, 8.46f);
        Vector3 pos3 = new Vector3(14.37f, 5.14f, -15.49f);
        Vector3 pos4 = new Vector3(-6.6f, 6.7f, 14f);
        Vector3 pos5 = new Vector3(-58.1f, 8.6f, -3.9f);
        Vector3[] initialPos = new Vector3[] { pos1, pos2, pos3, pos4, pos5 };

        GameObject Agent = GameObject.Find("Agent");
        trustValues = gameObject.GetComponent<InitializeTrust>().gaussienne(InitializeScene.nbTrust, 5.0);
        GameObject currentAgent;

        for (int i = 0; i < 5; i++)
        {
            currentAgent = Instantiate(Agent);

            // Determine agent characteristics
            if (InitializeScene.nbMemory != 0)
            {
                memory = true;
                InitializeScene.nbMemory--;
            }
            else
                memory = false;

            if (InitializeScene.nbCourage != 0)
            {
                courage = true;
                InitializeScene.nbCourage--;
            }
            else
                courage = false;

            currentAgent.GetComponent<AgentCaracteristics>().ReceiveAgentParameters(memory, courage, (float) trustValues[i], i);
            currentAgent.transform.position = initialPos[i];
            currentAgent.tag = "agent";
        }

        Agent.SetActive(false);
    }

}
