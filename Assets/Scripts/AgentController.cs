using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text;
using System;


public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshHit hit;
    private float range = 10.0f;
    private Vector3 point;
    //public List<string> indices = new List<string>();
    public double confidence;
    public bool coupableTrouve = false;
    public int exchangeNumber;
    public int clueNumber;
    public GameObject clue;
    public static System.Random random = new System.Random();
    public List<GameObject> inventory = new List<GameObject>();
    StringBuilder allGameObjects = new StringBuilder();
    private List<float> temps_indice = new List<float>();
    public GameObject lampe;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        confidence = gaussienne(80, 40);
        lampe.name = "lampe";
    }

    System.Random rand = new System.Random(1);

    public double gaussienne(float mean, float standardDeviation) {
       
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double x =
                     mean + standardDeviation * randStdNormal;

        return x;
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
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

    void FreeWalk()
    {
        if (RandomPoint(transform.position, range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
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
                    Collider[] objectinrange = See(transform.position, seeRange);
                    SeeIndice(objectinrange);
                    SeeAgent(objectinrange);
                    SeeVillager(objectinrange);
                    if (temps_indice.Count > 0)
                    {
                        VerifTime();
                    }
                }
        }
    }

    public float seeRange = 4.0f;             //range to detect items

    public string IndiceTag = "indice";  //edible tag
    public string AgentTag = "agent";  //edible tag
    public string VillagerTagFL = "villagerFL";  //edible tag
    public string VillagerTagF = "villagerF";  //edible tag
    public string VillagerTagL = "villagerL";  //edible tag


    void SeeIndice(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (IndiceTag == objectinrange[i].tag)
                {
                    Debug.Log("Detected :" + objectinrange[i].name);
                    if (!agent.gameObject.GetComponent<AgentCaracteristics>().indices.Contains(objectinrange[i].gameObject))
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().indices.Add(objectinrange[i].gameObject);
                        
                        // temps au moment trouvaille
                        temps_indice.Add(Time.deltaTime);
                        inventory.Add(objectinrange[i].gameObject); // plus facile pour la mémoire de garder aussi cet inventaire

                        //pour l'inventaire
                        allGameObjects.Append(objectinrange[i].name + "\n");
                    }
                }
            }
        }
    }

    //get a list of colliders in range on this layer
    Collider[] See(Vector3 agentPosition, float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(agentPosition, range);
        return hitColliders;
    }

    void SeeAgent(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (AgentTag == objectinrange[i].tag) //on vérifie que l'objet détecté est bien un agent
                {
                    Debug.Log("Detected : " + objectinrange[i].name);

                    if ((objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().indices != null) && (objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().indices.Count != 0)) //A VERIFIER
                    {
                        List<GameObject> agentMetIndices = objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().indices;

                        //récupérer trust de l'agent
                        int trust = 101; //A REMPLACER

                        //random => if random < trust, l'agent reçoit l'un des objets de l'autre
                        exchangeNumber = random.Next(0, 101);
                        if (exchangeNumber < trust)
                        {
                            //récupérer l'inventaire de l'autre agent, sélectionner un objet au hasard
                            clueNumber = random.Next(0, agentMetIndices.Count);
                            GameObject objet = agentMetIndices[clueNumber];

                            // vérification de l'inventaire, si objet déjà présent -> on ne l'ajoute pas
                            if (!agent.gameObject.GetComponent<AgentCaracteristics>().indices.Contains(objet))
                            {
                                agent.gameObject.GetComponent<AgentCaracteristics>().indices.Add(objet);
                                // temps au moment trouvaille
                                temps_indice.Add(Time.deltaTime);
                                inventory.Add(objectinrange[i].gameObject); // plus facile pour la mémoire de garder aussi cet inventaire

                                //pour l'inventaire
                                allGameObjects.Append(objectinrange[i].name + "\n");
                            }
                        }
                    }

                }
            }
        }
    }

    void SeeVillager(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (VillagerTagFL == objectinrange[i].tag)
                {
                    Debug.Log("Detected :" + objectinrange[i].name);
                    if (!agent.gameObject.GetComponent<AgentCaracteristics>().indices.Contains(lampe))
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().indices.Add(lampe);
                    }
                }
                if (VillagerTagF == objectinrange[i].tag)
                {
                    Debug.Log("Detected :" + objectinrange[i].name);
                    //si courage, agent va dans la forêt
                }
                if (VillagerTagL == objectinrange[i].tag)
                {
                    Debug.Log("Detected :" + objectinrange[i].name);
                    int trust = agent.gameObject.GetComponent<AgentCaracteristics>().trust; //A CHANGER

                    //random => if random < trust et trust élevée, l'agent croit le villageois et oublie tous ses indices
                    exchangeNumber = random.Next(0, 101);
                    if (exchangeNumber < trust && trust > 85)
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().indices.Clear();
                    }
                }
            }
        }
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

            GUI.BeginGroup(new Rect(0, 0, 2000, 2000));

            Rect sizeBox = new Rect(950, 10, 150, 500);
            GUI.Box(sizeBox, "\n \n Indices récupérés : \n \n" + allGameObjects);

            if (GUI.Button(new Rect(950, 10, 30, 30), "X"))
            {
                InventoryOn = false;
            }

            GUI.EndGroup();
        }
    }

    /// <summary>
    /// COUPABLE DESIGNE
    /// </summary>

    public bool Coupable()
    {
        string[] indices = { "couteau", "champignons", "compas", "briquet", "carnet", "lampe" };
        int[] points = { 6, 5, 4, 3, 2, 1 };
        int total = 0;


        for (int i = 0; i < inventory.Count; i++)
        {
            for (int j = 0; j < indices.Length; j++)
            {
                if (inventory[i].name == indices[j])
                {
                    total = total + points[j];
                }

            }
        }

        if (total >= 10)
            return true;
        else return false;
    }


    /// <summary>
    /// MEMOIRE
    /// </summary>
    /// 


    private float timer = 0.0f;
    private List<string> anciens_indices=new List<string>();

    void VerifTime() // il faudrait le faire pour deux niveaux différents (là on suppose que c'est pour la bonne mémoire)
    {
        timer += Time.deltaTime;
        int k = 0;
        
        
        for (int i = 0; i < temps_indice.Count; i++)
        {

            for (int j = 0; j < anciens_indices.Count; j++)
            {
                Debug.Log("verification temps");
                if (inventory[i].name == anciens_indices[j])
                {
                    k++; //compte le nombre de rappels  
                }

                if (k == 0) //il a eu l'indice qu'une seule fois
                {
                    if (timer - temps_indice[i] > 100.0f) // bon là j'ai mit un temps au hasard, à redéfinir
                    {
                       inventory.Remove(inventory[i]);
                        agent.gameObject.GetComponent<AgentCaracteristics>().indices.Remove(inventory[i].name);
                        temps_indice.Remove(temps_indice[i]);
                        anciens_indices.Add(inventory[i].name);

                    }
                }
                else
                {
                    if (timer - temps_indice[i] > 200.0f) // s'en souvient plus longtemps s'il y a eu un rappel
                    {
                        inventory.Remove(inventory[i]);
                        agent.gameObject.GetComponent<AgentCaracteristics>().indices.Remove(inventory[i].name);
                        temps_indice.Remove(temps_indice[i]);
                        anciens_indices.Add(inventory[i].name);

                    }
                }

                k = 0;
            }



        }


    }

}

