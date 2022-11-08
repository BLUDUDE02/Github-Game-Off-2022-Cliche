using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    public string favoriteFood;
    public string HeadColor;
    public string BodyColor;
    
    public float height;
    public GameObject Head;
    public GameObject Body;

    Color colorHead;
    Color colorBody;

    private void Awake()
    {
        colorHead = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        float r = colorHead.r;
        float b = colorHead.b;
        float g = colorHead.g;

        if (r > b && r > g)
            HeadColor = "red";
        else if (b > r && b > g)
            HeadColor = "blue";
        else if (g > r && g > b)
            HeadColor = "green";
        else
            HeadColor = null;

        colorBody = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        r = colorBody.r;
        b = colorBody.b;
        g = colorBody.g;

        if (r > b && r > g)
            BodyColor = "red";
        else if (b > r && b > g)
            BodyColor = "blue";
        else if (g > r && g > b)
            BodyColor = "green";
        else
            BodyColor = null;

        Head.transform.GetComponent<Renderer>().material.color = colorHead;
        Body.transform.GetComponent<Renderer>().material.color = colorBody;
    }
}
