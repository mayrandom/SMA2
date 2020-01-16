using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text;

public class control_courage : MonoBehaviour
{

    void Start()
    {

    }

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
                    Debug.Log("Detected :" + agentinrange[i].name);
                    Debug.Log("courage :" + agentinrange[i].gameObject.GetComponent<AgentCaracteristics>().courage);


                    if (agentinrange[i].gameObject.GetComponent<AgentCaracteristics>().courage == false)
                    {

                        Debug.Log("OUST");

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