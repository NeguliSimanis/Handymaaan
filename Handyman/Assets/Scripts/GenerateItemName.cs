using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItemName
{

    //Item.EquippedSlot 

    static string[] prefixes =
    {
        "Bloody",
        "Crooked",
        "Broken",
        "Beautiful",
        "Blue-haired",
        "Mysterious",
        "Dirty",
    };

    static string[] legNames =
    {
        "Leg",
        "Paw",
        "Foot",
        "Ankle"
    };
    static string[] lastNames =
    {
        "of Seer",
        "of Vigilance",
        "of the Prophet",
        "of Gamejams"
    };

    static string[] armNames =
    {
        "Biceps",
        "Arm",
        "Strongarm",
        "Right Hook",
        "Hand",
        "Claw",
    };

    static string[]headNames =
    {
        "Head",
        "Face",
        "Scalp",
        "Skull"
    };
   
    public static string GenerateLimbName(Item.EquippedSlot limbType)
    {
        string name = "default";
        if (limbType == Item.EquippedSlot.Head)
        {
            name = prefixes[Random.Range(0,prefixes.Length-1)] + " " + headNames[Random.Range(0, headNames.Length - 1)];
            if (Random.Range(0,1)>0.5f)
            {
                name = name + " " + lastNames[Random.Range(0, lastNames.Length - 1)];
            }
        }
        else if (limbType == Item.EquippedSlot.LeftHand || limbType == Item.EquippedSlot.RightHand)
        {
            name = prefixes[Random.Range(0, prefixes.Length - 1)] + " " + armNames[Random.Range(0, armNames.Length - 1)];
            if (Random.Range(0, 1) > 0.5f)
            {
                name = name + " " + lastNames[Random.Range(0, lastNames.Length - 1)];
            }
        }
        else if (limbType == Item.EquippedSlot.LeftLeg || limbType == Item.EquippedSlot.RightLeg)
        {
            name = prefixes[Random.Range(0, prefixes.Length - 1)] + " " + legNames[Random.Range(0, legNames.Length - 1)];
            if (Random.Range(0, 1) > 0.5f)
            {
                name = name + " " + lastNames[Random.Range(0, lastNames.Length - 1)];
            }
        }
        return name;
    }

    
}
