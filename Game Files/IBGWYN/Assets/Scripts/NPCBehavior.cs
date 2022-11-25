using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCBehavior : MonoBehaviour
{
    [Header("Settings")]
    public float wanderRadius;
    public float wanderTimer;
    public Color gizmoColor;
    public Animator anim;
    public ParticleSystem Explosion;
    public AudioSource source;


    private NavMeshAgent agent;
    private float timer;
    private float chairtimer;
    public bool tryingToSit = true;
    public bool CanSit;
    public bool CanTalk = true;
    GameManager gm;

    public GameObject[] friends;
    GameObject Target;

    int dialoguenum = 99;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        friends = GameObject.FindGameObjectsWithTag("NPC");
        source = GetComponent<AudioSource>();
        source.volume = 0;
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
        if (!CanSit)
        {
            Wander();
        }
        else
        {
            goToChair();
        }

        if (tryingToSit)
            findChair();

        anim.SetBool("Moving", Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance && !agent.isStopped ? true : false);
        anim.speed = agent.speed;

        if(!source.isPlaying)
            source.volume = 0;
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

        if (!source.isPlaying && CanTalk)
        {
            int i = Random.Range(0, 2);

            if (i < 2)
            {
                source.clip = (i == 0 ? GetComponent<DataDictionary>().Moving : GetComponent<DataDictionary>().Same);
                source.PlayOneShot(source.clip);
            }
        }
    }

    void findChair()
    {
        float smallestdist = 10;
        foreach (target t in gm.freechairs)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            Debug.Log(GetComponent<NPCData>().characterName + " is " + dist + " away from a chair");
            if (dist <= 5f)
            {                     
                if (dist <= smallestdist)
                {
                    if(!t.used)
                    {
                        smallestdist = dist;
                        Target = t.gameObject;
                        t.used = true;
                        CanSit = true;
                    }
                }
            }
        }
    }

    void goToChair()
    {
        chairtimer += Time.deltaTime;
        agent.SetDestination(Target.transform.position);

        if (timer >= 5)
        {
            StartCoroutine(waitToSit());
            timer = 0;
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                transform.position = Target.transform.position;
                transform.rotation = Target.transform.rotation;
                anim.SetBool("sitting", true);
                StartCoroutine(sit());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            source.volume = 0.5f;
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
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They really like  to hang out in the " + target.favoriteFood + ".";
            source.clip = GetSound(target.favoriteFood.ToLower());
        }
        else if (choice == 1)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They're pretty " + (target.height > 1 ? "short" : "tall") + ".";
            source.clip = GetSound((target.height > 1 ? "short" : "tall"));
        }
        else if (choice == 2)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They're wearing a " + (target.color2) + " shirt .";
            source.clip = GetSound(target.color2.ToLower());
        }
        else
        {
            answer.text = "Oh, I don't know who that is...";
            source.clip = GetSound("NOTHING");
        }
        source.PlayOneShot(source.clip);
        return answer;

    }

    AudioClip GetSound(string x)
    {
        DataDictionary dictionary = GetComponent<DataDictionary>();
        switch (x)
        {
            case "red":
                return dictionary.RED[Random.Range(0, 3)];
            case "green":
                return dictionary.GREEN[Random.Range(0, 3)];
            case "yellow":
                return dictionary.YELLOW[Random.Range(0, 3)];
            case "magenta":
                return dictionary.MAGENTA[Random.Range(0, 3)];
            case "black":
                return dictionary.BLACK[Random.Range(0, 3)];
            case "blue":
                return dictionary.BLUE[Random.Range(0, 3)];
            case "cyan":
                return dictionary.CYAN[Random.Range(0, 3)];
            case "white":
                return dictionary.WHITE[Random.Range(0, 3)];
            case "short":
                return dictionary.Short[Random.Range(0, 2)];
            case "tall":
                return dictionary.Tall[Random.Range(0, 2)];
            case "main floor":
                return dictionary.Office;
            case "bathroom":
                return dictionary.Bathroom;
            case "conference room":
                return dictionary.Conference;
            case "break room":
                return dictionary.BreakRoom;
            default:
                return dictionary.IDontKnow[Random.Range(0, 2)];
        }
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
        if (!source.isPlaying && CanTalk)
        {
            if (Target.transform.parent.name.Contains("Chair"))
            {
                source.clip = GetComponent<DataDictionary>().Workin;
            }
            else
            {
                source.clip = GetComponent<DataDictionary>().Pooping[Random.Range(0, 2)];
            }
            source.PlayOneShot(source.clip);
        }

        yield return new WaitForSeconds(Random.Range(3, 5));
        anim.SetBool("sitting", false);
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        CanSit = false;
        StartCoroutine(waitToSit());
    }

    IEnumerator waitToSit()
    {
        agent.isStopped = false;
        CanSit = false;
        yield return new WaitForSeconds(10f);
        tryingToSit = true;
        Target.GetComponent<target>().used = false;
        Target.GetComponent<target>().NPC = null;
    }

    IEnumerator waitToTalk()
    {
        CanSit = false;
        yield return new WaitForSeconds(10f);
        CanSit = true;
    }

    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, wanderRadius);
    }
}
