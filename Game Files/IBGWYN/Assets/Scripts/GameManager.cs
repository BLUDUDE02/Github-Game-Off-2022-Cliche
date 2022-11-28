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
    public PlayerMovement player;
    public AudioSource Boss;
    public AudioClip clip;
    public AudioClip[] clips;
    public Camera cam;
    public GameObject gun;


    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.GetInt("TimesPlayed") == 0)
        {
            clip = clips[0];
        }
        else
        {
            clip = clips[Random.Range(1, 3)];
        }
        PlayerPrefs.SetInt("TimesPlayed", 1);
        PlayerPrefs.Save();
        player = FindObjectOfType<PlayerMovement>();
        StartCoroutine(cutscene());

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

        if(!talking && NPCInRange.Count > 0)
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

    IEnumerator cutscene()
    {
        player.enabled = false;
        gun.SetActive(false);
        cam.GetComponent<MouseLook>().enabled = false;
        Boss.PlayOneShot(clip);
        if(clip == clips[0])
        {
            yield return new WaitForSeconds(clip.length - 26);
        }
        gun.SetActive(true);
        if (clip == clips[0])
        {
            yield return new WaitForSeconds(26);
        }
        else
        {
            yield return new WaitForSeconds(5);
        }
        player.enabled = true;
        cam.GetComponent<MouseLook>().enabled = true;
        for (int i = 0; i < Quantity; i++)
            Invoke("spawnInCircle", 0);
        StartCoroutine(setup());
    }
}
