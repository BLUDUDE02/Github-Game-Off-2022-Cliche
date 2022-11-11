using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    [Header("Settings")]
    public float range = 3;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI factText;
    public TextMeshProUGUI Subtitles;
    public GameManager gm;

    List<int> factsobtained = new List<int>();
    bool talk;
    bool won;

    public struct fact
    {
        public string text;
        public int factint;
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        factText.text = null;
        Subtitles.text = null;

    }
    private void Update()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitinfo, range))
        {
            if (hitinfo.transform.tag == "NPC")
            {
                talk = true;
            }
            else
                talk = false;

        }
        else
            talk = false;

        if (talk)
        {
            interactText.text ="talk [E]";
            if(Input.GetKeyDown(KeyCode.E))
            {
                
                NPCBehavior activeNPC = hitinfo.transform.GetComponentInParent<NPCBehavior>();
                fact Question = activeNPC.GenerateFact(gm.target);
                
                if(!factsobtained.Contains(Question.factint) && Question.factint < 4)
                {
                    factText.text += "\n * " + Question.text;
                    factsobtained.Add(Question.factint);
                }
                
                if(!won)
                {
                    Subtitles.text = Question.text;
                    StartCoroutine(ClearText());
                }
                    
            }
        }
        else
        {
            interactText.text = null;
        }
    }

    public void Won()
    {
        won = true;
        Subtitles.text = "Holy shit you killed " + gm.target.characterName + "!";
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(3f);
        Subtitles.text = null;
    }
}