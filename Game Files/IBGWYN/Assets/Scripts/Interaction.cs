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

    bool talk;

    private void Start()
    {
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
            interactText.text = hitinfo.transform.GetComponentInParent<NPCData>().characterName + " [E]";
            if(Input.GetKeyDown(KeyCode.E))
            {
                NPCData Target = hitinfo.transform.GetComponentInParent<NPCData>();
                NPCCommunication activeNPC = hitinfo.transform.GetComponentInParent<NPCCommunication>();
                string text = activeNPC.GenerateFact(Target);
                factText.text += text;
                Subtitles.text = text;
                StartCoroutine(ClearText());
            }
        }
        else
        {
            interactText.text = null;
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(3f);
        Subtitles.text = null;
    }
}
