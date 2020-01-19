using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Text;
using System;


public class AgentController : MonoBehaviour
{
    public static System.Random random = new System.Random();
    public static bool culpritFound = false;
    public List<GameObject> inventory = new List<GameObject>();
    public List<float> clueTimes = new List<float>();

    private NavMeshAgent agent;
    private NavMeshHit hit;
    private float range = 40.0f;
    private Vector3 point;    
    private int exchangeNumber;
    private int clueNumber;
    private GameObject clue; 
    private StringBuilder allGameObjects = new StringBuilder();
    private Vector3 forest = new Vector3(-51f, 3f, 15f);
    private Camera agentCamera;
    private GameObject lampe;
    public bool outForest = false;
    Vector3 direction = new Vector3(1, 0, 0);

    [SerializeField]
    private Animation anim;
    [SerializeField]
    public GameObject returnButton;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentCamera = gameObject.GetComponent<Camera>();
        lampe = GameObject.Find("lampe");
    }

    System.Random rand = new System.Random(1);


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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "forest" && gameObject.GetComponent<AgentCaracteristics>().courage == 0.2f)
            outForest = true;
        else
            outForest = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "forest")
            outForest = false;
    }


    void FreeWalk()
    {
        if (outForest)
            transform.Translate(direction * Time.deltaTime);
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
            anim.Play("m_run");
            
            if (agent.remainingDistance <= agent.stoppingDistance) // if agent doesn't move
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    FreeWalk();
                    Collider[] objectinrange = See(transform.position, seeRange);
                    SeeIndice(objectinrange);
                    SeeAgent(objectinrange);
                    SeeVillager(objectinrange);
                    if (clueTimes.Count > 0)
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

    /// <summary>
    /// Clue detection
    /// </summary>
    /// <param name="objectinrange"></param>
    void SeeIndice(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (IndiceTag == objectinrange[i].tag)
                {
                    if (!agent.gameObject.GetComponent<AgentCaracteristics>().clues.Contains(objectinrange[i].gameObject))
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().clues.Add(objectinrange[i].gameObject);
                        
                        // discover time
                        clueTimes.Add(Time.deltaTime);
                        inventory.Add(objectinrange[i].gameObject); // plus facile pour la mémoire de garder aussi cet inventaire

                        //inventory
                        allGameObjects.Append(objectinrange[i].name + "\n");
                    }
                }
            }
        }
    }

    /// <summary>
    /// get a list of colliders in range on this layer
    /// </summary>
    /// <param name="agentPosition"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    Collider[] See(Vector3 agentPosition, float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(agentPosition, range);
        return hitColliders;
    }

    /// <summary>
    /// Agent detection
    /// </summary>
    /// <param name="objectinrange"></param>
    void SeeAgent(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (AgentTag == objectinrange[i].tag) //on vérifie que l'objet détecté est bien un agent
                {

                    if ((objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().clues != null) && (objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().clues.Count != 0)) //A VERIFIER
                    {
                        List<GameObject> agentMetclues = objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().clues;
                        float trust = agent.gameObject.GetComponent<AgentCaracteristics>().trust;

                        // Get object from the encountered agent
                        float exchangeNumber = UnityEngine.Random.Range(0.0f, 0.9f);
                        if (exchangeNumber < trust)
                        {
                            // Get encountered agent inventory, and select random object to exchange
                            clueNumber = random.Next(0, objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().clues.Count);
                            GameObject objet = objectinrange[i].gameObject.GetComponent<AgentCaracteristics>().clues[clueNumber];

                            // Inventory check, if the object is already present, we don't add it to the inventory
                            if (!agent.gameObject.GetComponent<AgentCaracteristics>().clues.Contains(objet))
                            {
                                agent.gameObject.GetComponent<AgentCaracteristics>().clues.Add(objet);
                                // Discovering time
                                clueTimes.Add(Time.deltaTime);
                                inventory.Add(objet);

                                // Inventory
                                allGameObjects.Append(objet.name + "\n");
                            }
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// Villager detection and encounter 
    /// </summary>
    /// <param name="objectinrange"></param>
    void SeeVillager(Collider[] objectinrange)
    {
        if (objectinrange.Length > 0)
        {
            for (int i = 0; i < objectinrange.Length; i++)
            {
                if (VillagerTagFL == objectinrange[i].tag)
                {
                    if (!agent.gameObject.GetComponent<AgentCaracteristics>().clues.Contains(lampe))
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().clues.Add(lampe);

                        // dicovering time for memory
                        clueTimes.Add(Time.deltaTime);
                        inventory.Add(lampe); 

                        //inventory
                        allGameObjects.Append(lampe.name + "\n");
                    }
                }
                if (VillagerTagF == objectinrange[i].tag)
                {
                    if (agent.gameObject.GetComponent<AgentCaracteristics>().courage == 0.8f)
                    {
                        agent.SetDestination(forest); //si courage, agent va dans la forêt
                        agent.transform.LookAt(forest);
                    }
                }
                if (VillagerTagL == objectinrange[i].tag)
                {
                    float trust = agent.gameObject.GetComponent<AgentCaracteristics>().trust;

                    //random => if random < trust et trust élevée, l'agent croit le villageois et oublie tous ses clues
                    float exchangeNumber = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (exchangeNumber < trust && trust >= 0.8f)
                    {
                        agent.gameObject.GetComponent<AgentCaracteristics>().clues.Clear();
                        inventory.Clear();
                    }
                }
            }
        }
    }

   

    /// <summary>
    /// INVENTORY
    /// </summary>

    //Private Variables
    private bool InventoryOn = false;

    void OnMouseUp()
    {
        //Enable agent camera and button for returning to main view
        agentCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Minimap"));
        agentCamera.enabled = true;
        returnButton.SetActive(true);
        returnButton.GetComponent<ReturnMainCamOnClick>().ReceiveAgent(gameObject);

        // Inventory display when left clicking on the agent
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

            Rect sizeBox = new Rect(930, 10, 200, 500);
            string memory = "mauvaise";
            string courage = "non";
            double confiance = agent.gameObject.GetComponent<AgentCaracteristics>().trust;
            if (agent.gameObject.GetComponent<AgentCaracteristics>().memory == 0.8f)
            { memory = "bonne"; }
            if (agent.gameObject.GetComponent<AgentCaracteristics>().courage == 0.8f)
            { courage = "oui"; }
            GUI.Box(sizeBox, "\n \n CARACTERISTIQUES : \n \n Confiance aux autres : " + confiance + "% \n Mémoire : " + memory + "\n Courageux : " + courage + "\n \n clues : \n \n" + allGameObjects);


            if (GUI.Button(new Rect(930, 10, 30, 30), "X"))
            {
                InventoryOn = false;
            }

            GUI.EndGroup();
        }
    }

    /// <summary>
    /// Found Culprit
    /// </summary>

    public void Culprit()
    {
        string[] clues = { "couteau", "champignons", "compas", "briquet", "carnet", "lampe" };
        int[] points = { 6, 5, 4, 3, 2, 1 };
        int total = 0;


        for (int i = 0; i < inventory.Count; i++)
        {
            for (int j = 0; j < clues.Length; j++)
            {
                if (inventory[i].name == clues[j])
                {
                    total = total + points[j];
                }

            }
        }

        if (total >= 10)
        {
            culpritFound = true;
        }
            
        else culpritFound = false;
    }


    /// <summary>
    /// MEMORY
    /// </summary>
    /// 


    private float timer = 0.0f;
    private List<string> oldClues = new List<string>();

    void VerifTime()
    {
        float timer = Time.timeSinceLevelLoad;
        int k = 0;

        for (int i = 0; i < clueTimes.Count; i++) // list update in SeeIndice or SeeAgent
        {
            for (int j = 0; j < oldClues.Count; j++)
            {

                if (agent.gameObject.GetComponent<AgentCaracteristics>().memory == 0.8f) //Good memory
                {
                    if (k == 0) // has got the clue only one time
                    {
                        if (timer - clueTimes[i] > 10.0f) 
                        {
                            oldClues.Add(inventory[i].name);
                            inventory.Remove(inventory[i]);
                            agent.gameObject.GetComponent<AgentCaracteristics>().clues.Remove(agent.gameObject.GetComponent<AgentCaracteristics>().clues[i]);
                            clueTimes.Remove(clueTimes[i]);


                        }
                    }
                    else
                    {
                        if (inventory[i].name == oldClues[j])
                        {
                            k++; //number of reminders
                        }

                        if (timer - clueTimes[i] > 150.0f) // longer memory if has got reminders
                        {
                            oldClues.Add(inventory[i].name);
                            inventory.Remove(inventory[i]);
                            agent.gameObject.GetComponent<AgentCaracteristics>().clues.Remove(agent.gameObject.GetComponent<AgentCaracteristics>().clues[i]);
                            clueTimes.Remove(clueTimes[i]);


                        }
                    }
                }

                if (agent.gameObject.GetComponent<AgentCaracteristics>().memory == 0.2f) //Bad memory
                {
                    if (k == 0) //got clue only one time
                    {
                        Debug.Log(i);
                        if (timer - clueTimes[i] > 10.0f) 
                        {
                            oldClues.Add(inventory[i].name);
                            inventory.Remove(inventory[i]);
                            agent.gameObject.GetComponent<AgentCaracteristics>().clues.Remove(agent.gameObject.GetComponent<AgentCaracteristics>().clues[i]);
                            clueTimes.Remove(clueTimes[i]);

                        }
                    }
                    else
                    {
                        if (timer - clueTimes[i] > 100.0f) // memory longer if there was a reminder
                        {
                            oldClues.Add(inventory[i].name);
                            inventory.Remove(inventory[i]);
                            agent.gameObject.GetComponent<AgentCaracteristics>().clues.Remove(agent.gameObject.GetComponent<AgentCaracteristics>().clues[i]);
                            clueTimes.Remove(clueTimes[i]);

                        }
                    }
                }

                k = 0;
            }
        }
    }
}

