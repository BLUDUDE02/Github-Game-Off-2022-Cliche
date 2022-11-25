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
    public NPCData target;
    public TextMeshProUGUI bullet;
    public List<target> chairs;
    public List<target> freechairs;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<Quantity; i++)
            Invoke("spawnInCircle",0);
        StartCoroutine(setup());

        target[] listx = FindObjectsOfType<target>();
        foreach(target t in listx)
        {
            chairs.Add(t);
            freechairs.Add(t);
        }
    }

    void spawnInCircle()
    {
        Vector3 pos = new Vector3((Random.insideUnitCircle * spawnRadius).x + transform.position.x, 0, (Random.insideUnitCircle * spawnRadius).y + transform.position.y);
        GameObject Character = Instantiate(NPC, pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        NPCs.Add(Character.GetComponent<NPCData>());
    }

    IEnumerator setup()
    {
        yield return new WaitForSeconds(0.3f);
        target = NPCs[0];
        target.isTarget = true;
        bullet.text = target.characterName;
    }

    private void Update()
    {
        chairManage();
    }

    void chairManage()
    {
        foreach(target t in freechairs)
        {
            if(t.used)
            {
                freechairs.Remove(t);
            }
        }
        foreach (target t in chairs)
        {
            if (!t.used && !freechairs.Contains(t))
            {
                freechairs.Add(t);
            }
        }
    }
}
