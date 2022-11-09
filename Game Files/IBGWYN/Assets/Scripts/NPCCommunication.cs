using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCommunication : MonoBehaviour
{
    string[] starters = { "Oh yeah I know ", "Let me tell ya about ", "Here's what I know about "};
    public string GenerateFact(NPCData target)
    {
        int choice = Random.Range(0, 3);
        if(choice == 0)
        {
            return starters[Random.Range(0, starters.Length)] + target.characterName + ". They really like " + target.favoriteFood + ".";
        }
        else if (choice == 1)
        {
            return starters[Random.Range(0, starters.Length)] + target.characterName + ". They're pretty " + (target.height > 1 ? "short" : target.height == 1 ? "average height" : "tall") + ".";
        }
        else
        {
            return "Oh, I don't know who that is...";
        }
        
    }
}
