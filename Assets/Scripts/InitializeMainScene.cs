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
        Vector3[] initialPos = new Vector3[] { pos1, pos2, pos3, pos4 };

        noms = new string[] { "Jean", "Dominique", "Michel", "Marie", "Pascal" };
        GameObject Agent = GameObject.Find("Agent");
        trustValues = gameObject.GetComponent<InitializeTrust>().gaussienne(InitializeScene.nbTrust, 5.0);


        for (int i = 1; i < 5; i++)
        {
            Instantiate(Agent);

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

            Agent.GetComponent<AgentCaracteristics>().ReceiveAgentParameters(memory, courage, (float) trustValues[i], i);
            Agent.transform.position = initialPos[i - 1];
        }
    }

}
