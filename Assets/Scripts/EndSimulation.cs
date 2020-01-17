﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSimulation : MonoBehaviour
{
    private GameObject[] agents;
    private int nbCulprit;

    [SerializeField]
    public GameObject nbAgentCourage;
    [SerializeField]
    public GameObject nbAgentMemory;
    [SerializeField]
    public GameObject meanAgentTrust;
    [SerializeField]
    public GameObject nbAgentCulprit;

    // Start is called before the first frame update
    void Start()
    {
        nbCulprit = 0;
        agents = GameObject.FindGameObjectsWithTag("agent");
        for(int i = 0; i < agents.Length; i++)
        {
            // voir les agents qui ont trouvé le coupable
        }

        nbAgentCulprit.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents ayant trouvé le coupable : " + nbCulprit;
        nbAgentCourage.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents courageux : " + InitializeScene.nbCourage;
        nbAgentMemory.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents ayant bonne mémoire : " + InitializeScene.nbMemory;
        meanAgentTrust.GetComponent<TextMeshProUGUI>().text = "Moyenne de confiance des agents : " + InitializeScene.nbTrust;
    }
}
