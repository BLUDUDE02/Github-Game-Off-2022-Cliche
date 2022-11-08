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

    bool talk;

    private void Start()
    {
        factText.text = null;
    }
    private void Update()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitinfo, range))
        {
            if (hitinfo.transform.tag == "NPC")
            {
                Debug.Log(hitinfo.transform.name);
                talk = true;
            }
            else
                talk = false;

        }
        else
            talk = false;

        if (talk)
        {
            interactText.text = hitinfo.transform.name + " [E]";
            if(Input.GetKeyDown(KeyCode.E))
            {
                NPCData Target = new NPCData();
                NPCCommunication activeNPC = hitinfo.transform.GetComponentInParent<NPCCommunication>();
                factText.text += activeNPC.GenerateFact(Target);
            }
        }
        else
        {
            interactText.text = null;
        }
    }
}
