using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCRoutines : MonoBehaviour
{
    int currentAction = 5;
    int dialogueNum = 99;

    bool sittingOnChair;
    bool sittingOnToilet;
    bool walking;

    Animator anim;
    ParticleSystem Explosion;
    AudioSource source;
    NavMeshAgent agent;
    GameManager gameManager;
    target T;
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

        NextAction();
    }
    #endregion

    #region Helper Functions
    //Selecting Next Action
    void NextAction()
    {
        int i = Random.Range(0, 5);

        HeadToNextActivity(i);
    }
    //Navigate Between
    void HeadToNextActivity(int i)
    {
        findTarget(i);
        if(T != null)
        {
            Navigate();
        }
        else
        {
            StartCoroutine(acting(3));
        }
    }
    void findTarget(int i)
    {
        switch(i)
        {
            case 0:
                foreach(target t in gameManager.OfficeChairs)
                {
                    if(t.NPC == null)
                    {
                        T = t;
                        break;
                    }
                        
                }
                break;
            case 1:
                foreach (target t in gameManager.Toilets)
                {
                    if (t.NPC == null)
                    {
                        T = t;
                        break;
                    }
                }
                break;
            default:
                foreach (target t in gameManager.Tables)
                {
                    if (t.NPC == null)
                    {
                        T = t;
                        break;
                    }
                }
                break;
        }
        if(T != null)
        {
            if (T.NPC == null)
            {
                T.NPC = this.gameObject;
                currentAction = i;
            }
            else T = null;
        }
    }
    //Moving
    void Navigate()
    {
        agent.SetDestination(T.transform.position);
    }

    void PerformTask()
    {
        if(T != null)
        {
            agent.isStopped = true;
            transform.position = T.transform.position;
            transform.rotation = T.transform.rotation;

            if (currentAction < 2) anim.SetBool("sitting", true);
            else anim.SetBool("Moving", false);
        }
        StartCoroutine(acting(Random.Range(8, 15)));
    }

    private void Update()
    {
        anim.SetBool("Moving", Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance && !agent.isStopped ? true : false);
        anim.speed = agent.speed;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            PerformTask();
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
        agent.isStopped = true;
        yield return new WaitForSeconds(source.clip != null ? source.clip.length : 3f);
        agent.isStopped = false;
        StartCoroutine(acting(Random.Range(4, 10)));
    }

    IEnumerator acting(int x)
    {
        yield return new WaitForSeconds(x);
        NextAction();
    }
    #endregion
}
