using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courage : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DetectAgent();
    }


    void DetectAgent()
    {
        float seeRange = 50.0f;             //range to detect items

        Collider[] agentinrange = DetectAgent(transform.position, seeRange);


        if (agentinrange.Length > 0)
        {
            for (int i = 0; i < agentinrange.Length; i++)
            {
                if ("agent" == agentinrange[i].tag)
                {
                    if (agentinrange[i].gameObject.GetComponent<AgentCaracteristics>().courage == 0.2f)
                    {
                        agentinrange[i].gameObject.GetComponent<AgentController>().transform.Rotate(Vector3.left * Time.deltaTime);
                    }


                }
            }
        }
    }

    Collider[] DetectAgent(Vector3 agentPosition, float range)
    {

        Collider[] hitColliders = Physics.OverlapSphere(agentPosition, range);
        return hitColliders;
    }
}
