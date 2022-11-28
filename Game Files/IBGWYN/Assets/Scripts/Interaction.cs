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
    public ParticleSystem win;
    public Camera cam;

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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, range))
        {
            if (hitinfo.transform.CompareTag("NPC"))
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
                
                NPCRoutines activeNPC = hitinfo.transform.GetComponentInParent<NPCRoutines>();
                fact Question = activeNPC.GenerateFact(gm.target);
                
                if(!factsobtained.Contains(Question.factint) && Question.factint < 3)
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
        win.Play();
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(3f);
        Subtitles.text = null;
    }
}
