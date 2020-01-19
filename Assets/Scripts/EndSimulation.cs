using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSimulation : MonoBehaviour
{
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
        nbAgentCulprit.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents ayant trouvé le coupable : " + UIParameters.nbCulprit;
        nbAgentCourage.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents courageux : " + InitializeScene.startNbCourage;
        nbAgentMemory.GetComponent<TextMeshProUGUI>().text = "Nombre d'agents ayant bonne mémoire : " + InitializeScene.startNbMemory;
        meanAgentTrust.GetComponent<TextMeshProUGUI>().text = "Moyenne de confiance des agents : " + InitializeScene.nbTrust;
    }
}
