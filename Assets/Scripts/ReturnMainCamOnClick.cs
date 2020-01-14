using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMainCamOnClick : MonoBehaviour
{
    GameObject agent;

    public void ReceiveAgent(GameObject activeAgent)
    {
        agent = activeAgent;
    }

    public void ReturnOnclick()
    {
        agent.GetComponent<Camera>().enabled = false;
        gameObject.SetActive(false);
    }
}
