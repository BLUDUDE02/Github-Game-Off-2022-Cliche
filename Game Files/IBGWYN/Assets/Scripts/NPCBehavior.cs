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


    private NavMeshAgent agent;
    private float timer;

    int dialoguenum = 99;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    string[] starters = { "Oh yeah I know ", "Let me tell ya about ", "Here's what I know about " };
    public Interaction.fact GenerateFact(NPCData target)
    {
        Interaction.fact answer;
        StartCoroutine(stopForASec());
        int choice = Random.Range(0, 6);
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
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They have a " + (target.color1) + " head .";
        }
        else if (choice == 3)
        {
            answer.text = starters[Random.Range(0, starters.Length)] + target.characterName + ". They have a " + (target.color2) + " body .";
        }
        else
        {
            answer.text = "Oh, I don't know who that is...";
        }
        return answer;

    }

    public void Die(Vector3 pos)
    {
        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform p in parts)
        {
            if(p.GetComponent<MeshRenderer>())
            {
                Rigidbody rb = p.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.AddExplosionForce(1000, pos, 0.1f);
                rb.transform.tag = "Object";
            }
            
        }
        Invoke("Delete", 0.1f);
        
    }

    void Delete()
    {
        Destroy(transform.GetComponent<NavMeshAgent>());
        Destroy(transform.GetComponent<NPCBehavior>());
        Destroy(transform.GetComponent<NPCData>());
    }

    IEnumerator stopForASec()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
    }

    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, wanderRadius);
    }
}
