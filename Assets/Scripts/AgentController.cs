using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text;


public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshHit hit;
    private float range = 10.0f;
    private Vector3 point;
    public List<GameObject> inventory = new List<GameObject>();
    StringBuilder allGameObjects = new StringBuilder();
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
    }

    //Choose th point where moving agents randomly
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    // Move agents
    void FreeWalk()
    {
        if (RandomPoint(transform.position, range, out point))
        {
            agent.SetDestination(point);
            agent.transform.LookAt(point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance) // si l'agent est immobile
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    FreeWalk();
                    SeeIndice();
                }
        }
    }

    public float seeRange = 1.0f;             //range to detect items
    public int layer = 2;                     //example scene indice layer
    public string IndiceTag = "indice";  //clue tag

    void SeeIndice()
    {
       
        Collider[] indiceinrange = SeeIndice(transform.position, seeRange, layer);
        if (indiceinrange.Length > 0)
        {
            for (int i = 0; i < indiceinrange.Length; i++)
            {
                if (IndiceTag == indiceinrange[i].tag)
                {
                    Debug.Log("Detected :" + indiceinrange[i].name);

                    if (!inventory.Contains(indiceinrange[i].gameObject))
                    {
                       inventory.Add(indiceinrange[i].gameObject);

                        allGameObjects.Append(indiceinrange[i].name + "\n");
                    }
                }
            }
        }     
    }

    //get a list of colliders in range on this layer
    Collider[] SeeIndice(Vector3 agentPosition, float range, int layer)
    {
       
        Collider[] hitColliders = Physics.OverlapSphere(agentPosition, range);
        return hitColliders;
    }

    /// <summary>
    /// INVENTAIRE
    /// </summary>

    //Private Variables
    private bool InventoryOn = false;
     
    void OnMouseUp()
    {
        //lorsqu on clic gauche sur l'objet contenant ce script,l'inventaire s'affiche et se ferme
        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryOn == false)
            {
                InventoryOn = true;
            }
           else if (InventoryOn == true)
            {
                InventoryOn = false;
            }
        }
    }
    void OnGUI()
    {
        if (InventoryOn == true)
        {           
            GUI.BeginGroup(new Rect(0,0, 2000,2000));
            
            Rect sizeBox = new Rect(950,10,150, 500);
            GUI.Box(sizeBox, "\n \n Indices récupérés : \n \n" + allGameObjects);

            if (GUI.Button(new Rect(950, 10, 30, 30), "X"))
            {
                InventoryOn = false;
            }

            GUI.EndGroup();
        }
    }

}

