using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentCaracteristics : MonoBehaviour
{
    public float memory;
    public float courage;
    public float trust;
    public List<GameObject> indices = new List<GameObject>();
    public string[] noms;
    public int id = 0;


    // Initialize agent
    // Initialize agent
    public void ReceiveAgentParameters(bool Memory, bool Courage, float Trust, int Id)
    {
        id = Id;
        trust = Trust;
        if (Memory)
            memory = 0.8f;
        else
            memory = 0.2f;

        if (Courage)
            courage = 0.8f;
        else
            courage = 0.2f;
    }

}

