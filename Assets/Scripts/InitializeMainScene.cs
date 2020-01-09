using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMainScene : MonoBehaviour
{
    public static float nbMemory;
    public static float nbTrust;
    public static float nbCourage;

    private string[] noms;

    bool memory;
    bool trust;
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

        for (int i = 1; i < 5; i++)
        {
            Instantiate(Agent);

            // Determine agent characteristics
            if (nbMemory != 0)
            {
                memory = true;
                nbMemory--;
            }
            else
                memory = false;

            if (nbTrust != 0)
            {
                trust = true;
                nbTrust--;
            }
            else
                trust = false;

            if (nbCourage != 0)
            {
                courage = true;
                nbCourage--;
            }
            else
                courage = false;

            Agent.GetComponent<AgentCaracteristics>().ReceiveAgentParameters(memory, courage, trust, i);
            Agent.transform.position = initialPos[i - 1];
        }
    }

}
