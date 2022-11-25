using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataDictionary : MonoBehaviour
{
    public string[] Fnames = { "Pyscho", "Bridge", "LaLa", "Dimpling", "Big Guy", "Fifi",
        "Bandit", "Chubs", "Beast", "Slim", "Boomhauer", "MomBod", "Honeybun", "Dirty Harry",
        "Dud", "Turkey", "Elf", "Big Mac", "Sunny", "Kitty", "Dummy", "Winnie", "Cello", "Cricket",
        "Betty Boop", "Janitor", "Ms. Congeniality", "Bacon", "Silly Gilly", "Chance" };
    public string[] Lnames = { "Larson", "Gordon", "Malone", "Carpenter", "Mcclure", "Rasmussen",
        "Gilbert", "Irwin", "Hurley", "Villanueva", "Trevino", "Norris", "Fisher", "Mata", "Elliott",
        "Andrade", "Whitaker", "Riddle", "Bryan", "Poole", "Cabrera", "Friedman", "Neal", "Hatfield",
        "Stevens", "Bender", "Knapp", "Mcclain", "Gallegos", "Joyce" };
    public string[] Places = {"Break room", "Conference Room", "Main Floor", "Bathroom"};
    public string[] Colors = {"red", "yellow", "green", "blue", "cyan", "white", "black", "magenta"};
    public Texture[] BodyTextures;
    public AudioClip[] RED;
    public AudioClip[] YELLOW;
    public AudioClip[] GREEN;
    public AudioClip[] BLUE;
    public AudioClip[] CYAN;
    public AudioClip[] WHITE;
    public AudioClip[] BLACK;
    public AudioClip[] MAGENTA;
    public AudioClip[] Pooping;
    public AudioClip[] IDontKnow;
    public AudioClip[] Tall;
    public AudioClip[] Short;
    public AudioClip BreakRoom;
    public AudioClip Conference;
    public AudioClip Office;
    public AudioClip Bathroom;
    public AudioClip Bumping;
    public AudioClip Moving;
    public AudioClip OhNo;
    public AudioClip Same;
    public AudioClip Workin;
}
