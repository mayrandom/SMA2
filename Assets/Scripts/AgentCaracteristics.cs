using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentCaracteristics : MonoBehaviour
{
    public bool memory;
    public bool courage;
    public double trust;
    public List<GameObject> indices = new List<GameObject>();
    public string[] noms;
    public int id = 0;

    // Initialize agent
    public void ReceiveAgentParameters(bool Memory, bool Courage, double Trust, int Id)
    {
        memory = Memory;
        courage = Courage;
        trust = Trust;
        id = Id;
    }

}

