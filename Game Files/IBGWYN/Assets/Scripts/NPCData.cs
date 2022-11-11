using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    public string favoriteFood;
    public GameManager gm;
    public string color1;
    public string color2;

    public float height;
    public GameObject Head;
    public GameObject Body;

    public bool isTarget = false;

    Color colorHead;
    Color colorBody;

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
        height = Random.Range(0.75f, 1.3f);
        transform.localScale *= height;
    }

    void PickFaves()
    {
        DataDictionary dictionary = new DataDictionary();
        characterName = dictionary.Fnames[Random.Range(0, dictionary.Fnames.Length - 1)] + " " +
            dictionary.Lnames[Random.Range(0, dictionary.Lnames.Length - 1)];
        favoriteFood = dictionary.Foods[Random.Range(0, dictionary.Foods.Length - 1)];

        int a = Random.Range(0, dictionary.Colors.Length - 1);
        int b = Random.Range(0, dictionary.Colors.Length - 1);
        color1 = dictionary.Colors[a];
        color2 = dictionary.Colors[b];

        Head.GetComponent<Renderer>().material.color = toColor(a);
        Body.GetComponent<Renderer>().material.color = toColor(b);
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
            case 9:
                return Color.grey;
            default:
                return Color.white;
        }
    }
}