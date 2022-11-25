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
        favoriteFood = dictionary.Places[Random.Range(0, dictionary.Places.Length - 1)];

        int b = Random.Range(0, dictionary.Colors.Length - 1);
        color2 = dictionary.Colors[b];
        body = dictionary.BodyTextures[b];
        Body.GetComponentInChildren<Renderer>().material.SetTexture("_BaseMap", body);
    }
}