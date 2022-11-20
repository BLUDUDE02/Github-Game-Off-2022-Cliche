using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    public string favoriteFood;
    public GameManager gm;
    public string color2;
    public Texture body;
    public Texture head;

    public float height;
    public GameObject Head;
    public GameObject Body;

    public bool isTarget = false;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        Generate();
    }

    void Generate()
    {
        SetHeight();
        PickFaves();
    }

    void SetHeight()
    {
        height = Random.Range(0.9f, 1.1f);
        transform.localScale *= height;
    }

    void PickFaves()
    {
        DataDictionary dictionary = GetComponent<DataDictionary>();
        characterName = dictionary.Fnames[Random.Range(0, dictionary.Fnames.Length - 1)] + " " +
            dictionary.Lnames[Random.Range(0, dictionary.Lnames.Length - 1)];
        favoriteFood = dictionary.Foods[Random.Range(0, dictionary.Foods.Length - 1)];

        int b = Random.Range(0, dictionary.Colors.Length - 1);
        color2 = dictionary.Colors[b];

        body = dictionary.BodyTextures[b];
        Body.GetComponentInChildren<Renderer>().material.SetTexture("_BaseMap", body);
    }

    public Color toColor(int color)
    {
        switch (color)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.yellow;
            case 3:
                return Color.blue;
            case 4:
                return Color.cyan;
            case 5:
                return Color.green;
            case 6:
                return Color.white;
            case 7:
                return Color.black;
            case 8:
                return Color.magenta;
            default:
                return Color.white;
        }
    }
}