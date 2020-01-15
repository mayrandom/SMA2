using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCaracteristics : MonoBehaviour
{
    private float memory;
    private float courage;
    private float trust;
    private GameObject[] indices;
    public string[] noms;
    public int id = 0;
    public float[] culpritConfidence;

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

    void Update()
    {
        //après avoir mis à jour les indices, on change le % de confiance envers le coupable
        //check les taux de confiance
    }

}
