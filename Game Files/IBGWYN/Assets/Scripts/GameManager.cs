using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject NPC;
    public float spawnRadius = 5;
    public int Quantity = 1;
    public List<NPCData> NPCs = new List<NPCData>();
    public List<NPCData> NPCInRange = new List<NPCData>();
    public NPCData active;
    public NPCData target;
    public TextMeshProUGUI bullet;
    public List<target> OfficeChairs = new List<target>();
    public List<target> Toilets = new List<target>();
    public List<target> Tables = new List<target>();
    PlayerMovement player;


    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();

        for (int i = 0; i<Quantity; i++)
            Invoke("spawnInCircle",0);
        StartCoroutine(setup());

        target[] targets = FindObjectsOfType<target>();
        foreach(target t in targets)
        {
            if (t.type == 0) OfficeChairs.Add(t);
            else if (t.type == 1) Toilets.Add(t);
            else Tables.Add(t);
        }
    }

    void spawnInCircle()
    {
        Vector3 pos = new Vector3((Random.insideUnitCircle * spawnRadius).x + transform.position.x, 0, (Random.insideUnitCircle * spawnRadius).y + transform.position.y);
        GameObject Character = Instantiate(NPC, pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        NPCs.Add(Character.GetComponent<NPCData>());
    }

    private void Update()
    {
        bool talking = false;
        foreach(NPCData n in NPCs)
        {
            if(Vector3.Distance(n.transform.position, player.transform.position) <= 5)
            {
                if (!NPCInRange.Contains(n))
                {
                    NPCInRange.Add(n);
                }
                if (n.GetComponent<AudioSource>().isPlaying)
                {
                    talking = true;
                }
            }
            else
            {
                if(NPCInRange.Contains(n))
                {
                    NPCInRange.Remove(n);
                }
                if (n.GetComponent<AudioSource>().isPlaying)
                {
                    n.GetComponent<AudioSource>().Stop();
                }
            }

            
        }

        if(!talking)
        {
            getNPCThatCanSpeak();
        }


    }

    void getNPCThatCanSpeak()
    {
        active = NPCInRange[Random.Range(0, NPCInRange.Count)];
        active.GetComponent<NPCRoutines>().Speak();
    }

    IEnumerator setup()
    {
        yield return new WaitForSeconds(0.3f);
        target = NPCs[0];
        target.isTarget = true;
        bullet.text = target.characterName;
    }
}
