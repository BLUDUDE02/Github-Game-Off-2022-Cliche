using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCRoutines : MonoBehaviour
{
    public target T;

    public int currentAction = 0;
    int i = 0;
    int dialogueNum = 99;

    public bool isCoolToMove = true;
    public bool cr;

    Animator anim;
    ParticleSystem Explosion;
    AudioSource source;
    NavMeshAgent agent;
    GameManager gameManager;
    
    DataDictionary dictionary;

    #region Setup
    private void Awake()
    {
        Explosion = GetComponentInChildren<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
        dictionary = GetComponent<DataDictionary>();
    }
    #endregion

    #region Helper Functions
    //Selecting Next Action
    void NextAction()
    {
        isCoolToMove = false;
        agent.enabled = false;
        if(T != null) T.NPC = null;
        T = null;
        i = Random.Range(0, 5);
        if(i == currentAction)
        {
            i += 1;
        }
        HeadToNextActivity(i);
    }
    //Navigate Between
    void HeadToNextActivity(int i)
    {
        if (findTarget(i)) Navigate();
    }
    bool findTarget(int i)
    {
        switch (i)
        {
            case 0:
                foreach (target t in gameManager.OfficeChairs)
                {
                    if (t.NPC == null)
                    {
                        T = t;
                        T.NPC = this.gameObject;
                        currentAction = i;
                        Debug.Log(GetComponent<NPCData>().characterName + " has found a target");
                        return true;
                    }
                }
                break;
            case 1:
                foreach (target t in gameManager.Toilets)
                {
                    if (t.NPC == null)
                    {
                        T = t;
                        T.NPC = this.gameObject;
                        currentAction = i;
                        Debug.Log(GetComponent<NPCData>().characterName + " has found a target");
                        return true;
                    }
                }
                break;
            default:
                foreach (target t in gameManager.Tables)
                {
                    if (t.NPC == null)
                    {
                        T = t;
                        T.NPC = this.gameObject;
                        currentAction = i;
                        Debug.Log(GetComponent<NPCData>().characterName + " has found a target");
                        return true;
                    }
                }
                break;
        }
        Debug.Log(GetComponent<NPCData>().characterName + " has not found a target");
        return false;
    }
    
    //Moving
    void Navigate()
    {
        agent.enabled = true;
        agent.SetDestination(T.transform.position);
    }

    private void Update()
    {
        anim.SetBool("Moving", Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance && agent.enabled ? true : false);
        anim.speed = agent.speed;
    }

    private void FixedUpdate()
    {
        
        
        if (isCoolToMove)
        {
            NextAction();
        }
        if (agent.enabled && !agent.pathPending)
        {
            Vector3 destination = agent.destination;
            Debug.Log((agent.enabled) + " " + (Vector3.Distance(agent.destination, T.transform.position) <= 0.02f) + " " + (agent.remainingDistance <= agent.stoppingDistance + 0.02f));
            if (Vector3.Distance(agent.destination, T.transform.position) <= 0.02f && agent.remainingDistance <= agent.stoppingDistance + 0.02f)
            {
                agent.enabled = false;
                transform.position = T.transform.position;
                transform.rotation = T.transform.rotation;

                if (currentAction < 2) anim.SetBool("sitting", true);
                else anim.SetBool("Moving", false);
                if (!cr) StartCoroutine(acting(Random.Range(5, 10)));
            }
        }
        
    }

    public void Speak()
    {
        if(T != null && !anim.GetBool("Moving"))
        {
            if (T.type == 1 && anim.GetBool("sitting"))
            {
                source.PlayOneShot(dictionary.Pooping[Random.Range(0, 2)]);
            }
            else if (T.type == 0 && anim.GetBool("sitting"))
            {
                source.PlayOneShot(dictionary.Workin);
            }
        }
        else
        {
            int i = Random.Range(0, 2);
            if (i == 0) source.PlayOneShot(dictionary.Moving);
            else source.PlayOneShot(dictionary.Same);
        }
        
    }
    #endregion

    #region Interactions

    string[] starters = { "Oh yeah I know ", "Let me tell ya about ", "Here's what I know about " };
    public Interaction.fact GenerateFact(NPCData target)
    {
        Interaction.fact answer;
        StopAllCoroutines();
        source.Stop();
        StartCoroutine(stopForASec());
        int choice = Random.Range(0, 5);
        if (dialogueNum == 99)
        {
            dialogueNum = choice;
        }
        else
        {
            choice = dialogueNum;
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

    #region Death
    public void Die(Vector3 pos)
    {
        Invoke("Delete", 0.1f);
        Explosion.Play();
    }

    void Delete()
    {
        Explosion.gameObject.transform.parent = null;
        gameManager.NPCs.Remove(GetComponent<NPCData>());
        foreach (Component child in GetComponentsInChildren(typeof(Component)))
        {
            Destroy(child.gameObject);
        }

    }
    #endregion

    #region Coroutines
    IEnumerator stopForASec()
    {
        yield return new WaitForSeconds(source.clip != null ? source.clip.length : 3f);
        agent.enabled = true;
        StartCoroutine(acting(Random.Range(4, 10)));
    }

    IEnumerator acting(int x)
    {
        Debug.Log("acting() has been called for " + GetComponent<NPCData>().characterName);
        cr = true;
        yield return new WaitForSeconds(x);
        anim.SetBool("sitting", false);
        isCoolToMove = true;
        cr = false;
    }
    #endregion
}
