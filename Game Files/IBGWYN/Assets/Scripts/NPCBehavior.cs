using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCBehavior : MonoBehaviour
{
    [Header ("Settings")]
    public float wanderRadius;
    public float wanderTimer;
    public Color gizmoColor;
    public Animator anim;
    public ParticleSystem Explosion;


    private NavMeshAgent agent;
    private float timer;
    public bool tryingToSit = true;
    public bool CanSit;

    public GameObject[] chairs;
    GameObject Target;

    int dialoguenum = 99;

    private void Awake()
    {
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }
    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!CanSit)
        {
            Wander();
            if(tryingToSit)
                findChair();
        }
        else
        {
            goToChair();
        }

        


        anim.SetBool("Moving", Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance && !agent.isStopped ? true: false);
        anim.speed = agent.speed;
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void findChair()
    {
        float smallestdist = 10;
        foreach (GameObject t in chairs)
        {
            float dist = Vector3.Distance(transform.position, t.GetComponentInChildren<target>().transform.position);
            Debug.Log(GetComponent<NPCData>().characterName + " is " + dist + " away from a chair");
            if (dist <= 5f)
            {
                if (!t.GetComponentInChildren<target>().used)
                {
                    if (dist <= smallestdist)
                    {
                        if(Target != null)
                            Target.GetComponent<target>().used = false;
                        smallestdist = dist;
                        Target = t.GetComponentInChildren<target>().gameObject;
                        Target.GetComponent<target>().used = true;
                        CanSit = true;
                        tryingToSit = false;
                    }
                }
            }
        }     
    }

    void goToChair()
    {
        agent.SetDestination(Target.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            transform.position = Target.transform.position;
            transform.rotation = Target.transform.rotation;
            anim.SetBool("sitting", true);
            StartCoroutine(sit());
        }
    }
    private void LateUpdate()
    {
        if(!agent.isStopped)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    #region Interactions

    string[] starters = { "Oh yeah I know ", "Let me tell ya about ", "Here's what I know about " };
    public Interaction.fact GenerateFact(NPCData target)
    {
        Interaction.fact answer;
        StartCoroutine(stopForASec());
        int choice = Random.Range(0, 5);
        if(dialoguenum == 99)
        {
            dialoguenum = choice;
        }
        else
        {
            choice = dialoguenum;
        }

        answer.factint = choice;
        if (choice == 0)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They really like " + target.favoriteFood + ".";
        }
        else if (choice == 1)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They're pretty " + (target.height > 1 ? "short" : target.height == 1 ? "average height" : "tall") + ".";
        }
        else if (choice == 2)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They're wearing a " + (target.color2) + " shirt .";
        }
        else
        {
            answer.text = "Oh, I don't know who that is...";
        }
        return answer;

    }

    #endregion

    public void Die(Vector3 pos)
    {
        Invoke("Delete", 0.1f);
        Explosion.Play();
    }

    void Delete()
    {
        Explosion.gameObject.transform.parent = null;
        foreach (Component child in GetComponentsInChildren(typeof(Component)))
        {
            Destroy(child.gameObject);
        }
        
    }

    IEnumerator stopForASec()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
    }
    IEnumerator sit()
    {
        yield return new WaitForSeconds(Random.Range(3, 5));
        anim.SetBool("sitting", false);
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        CanSit = false;
        StartCoroutine(waitToSit());
    }

    IEnumerator waitToSit()
    {
        yield return new WaitForSeconds(10f);
        tryingToSit = true;
        Target.GetComponent<target>().used = false;
    }

    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, wanderRadius);
    }
}
