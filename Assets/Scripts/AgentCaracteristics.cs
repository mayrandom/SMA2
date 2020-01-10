using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentCaracteristics : MonoBehaviour
{
    private bool memory;
    public bool courage;
    public bool trust;
    public List<string> indices = new List<string>();
    public string[] noms;
    public int id = 0;

    // Initialize agent
    public void ReceiveAgentParameters(bool Memory, bool Courage, bool Trust, int Id)
    {
        memory = Memory;
        courage = Courage;
        trust = Trust;
        id = Id;
        indices.Add("test");
    }

}

