using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    public string favoriteFood;
    public GameManager gm;
    
    public float height;
    public GameObject Head;
    public GameObject Body;

    public bool isTarget;

    Color colorHead;
    Color colorBody;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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
        DataDictionary dictionary = new DataDictionary();
        characterName = dictionary.Fnames[Random.Range(0, dictionary.Fnames.Length - 1)] + " " +
            dictionary.Lnames[Random.Range(0, dictionary.Lnames.Length - 1)];
        favoriteFood = dictionary.Foods[Random.Range(0, dictionary.Foods.Length - 1)];
    }
}
