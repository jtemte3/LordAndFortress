using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCustomizationController : MonoBehaviour
{

    public List<GameObject> helmets = new();
    public List<Material> torsoMaterials = new();
    public List<GameObject> armors = new();
    public List<Material> legMaterials = new();
    public GameObject torso;
    public GameObject legs;

    int helmetPos = 0;
    int torsoPos = 0;
    int armorPos = 0;
    int legsPos = 0;

    public void NextHelmet()
    {
        helmets[helmetPos].SetActive(false);

        helmetPos = NextPos(helmetPos, helmets.Count);
        helmets[helmetPos].SetActive(true);
    }

    public void PreviousHelmet()
    {
        helmets[helmetPos].SetActive(false);

        helmetPos = PreviousPos(helmetPos, helmets.Count);
        helmets[helmetPos].SetActive(true);
    }

    public void NextArmor()
    {
        armors[armorPos].SetActive(false);

        armorPos = NextPos(armorPos, armors.Count);
        armors[armorPos].SetActive(true);
    }

    public void PreviousArmor()
    {
        armors[armorPos].SetActive(false);

        armorPos = PreviousPos(armorPos, armors.Count);
        armors[armorPos].SetActive(true);
    }
    public void NextTorso()
    {
        torsoPos = NextPos(torsoPos, torsoMaterials.Count);
        torso.GetComponent<Renderer>().material = torsoMaterials[torsoPos];
    }
    public void PreviousTorso()
    {
        torsoPos = PreviousPos(torsoPos, torsoMaterials.Count);
        torso.GetComponent<Renderer>().material = torsoMaterials[torsoPos];
    }
    public void NextLegs()
    {
        legsPos = NextPos(legsPos, legMaterials.Count);

        var mats = legs.GetComponent<Renderer>().sharedMaterials;
        mats[1] = legMaterials[legsPos];
        legs.GetComponent<Renderer>().sharedMaterials = mats;
    }
    public void PreviousLegs()
    {
        legsPos = PreviousPos(legsPos, legMaterials.Count);

        var mats = legs.GetComponent<Renderer>().sharedMaterials;
        mats[1] = legMaterials[legsPos];
        legs.GetComponent<Renderer>().sharedMaterials = mats;
    }

    private int NextPos (int currentPos, int listCount)
    {
        return (currentPos + 1) % listCount;
    }

    private int PreviousPos(int currentPos, int listCount)
    {
        if (currentPos == 0)
        {
            return listCount - 1;
        }
        else
        {
            return currentPos - 1;
        }
        
    }


}
