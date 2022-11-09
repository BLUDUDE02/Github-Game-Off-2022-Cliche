using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    public string favoriteFood;
    
    public float height;
    public GameObject Head;
    public GameObject Body;

    Color colorHead;
    Color colorBody;

    private void Awake()
    {
        Generate();
    }

    void Generate()
    {
        GenerateColors();
        SetHeight();
        PickFaves();
    }

    void GenerateColors()
    {
        colorHead = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        colorBody = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Head.transform.GetComponent<Renderer>().material.color = colorHead;
        Body.transform.GetComponent<Renderer>().material.color = colorBody;
    }

    void SetHeight()
    {
        height = Random.Range(0.75f, 1.3f);
        transform.localScale *= height;
    }

    void PickFaves()
    {
        favoriteFood = "corn";
    }
}
