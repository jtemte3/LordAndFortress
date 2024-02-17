using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCustomizationController : MonoBehaviour
{
    public FactionManager factionManager;
    public List<GameObject> helmets = new();
    public List<Material> torsoMaterials = new();
    public List<GameObject> armors = new();
    public List<Material> legMaterials = new();
    //public List<Color> FactonColors = new();
    public GameObject torso;
    public GameObject legs;

    int helmetPos = 0;
    int torsoPos = 0;
    int armorPos = 0;
    int legsPos = 0;
    //int colorPos = 0;

    private void Start()
    {
        if (torso.GetComponent<Renderer>().material.name.Contains("Cloth"))
        {
            torso.GetComponent<Renderer>().material.color = factionManager.currentColor;
        }
    }

    public CustomUnitObject ExportUnit()
    {
        CustomUnitObject currentUnit = new();
        currentUnit.helmetId = helmetPos;
        currentUnit.torsoId = torsoPos;
        currentUnit.armorId = armorPos;
        currentUnit.legsId = legsPos;
        currentUnit.weaponId = 0;
        currentUnit.unitGoldCost = 0;
        currentUnit.unitHealth = 10;
        currentUnit.unitSpeed = 3;

        return currentUnit;
    }

    public void LoadUnit(CustomUnitObject newUnit)
    {
        foreach (GameObject obj in helmets)
        {
            obj.SetActive(false);
        }

        helmetPos = newUnit.helmetId;
        helmets[helmetPos].SetActive(true);

        foreach (GameObject obj in armors)
        {
            obj.SetActive(false);
        }
        armorPos = newUnit.armorId;
        armors[armorPos].SetActive(true);

        torsoPos = newUnit.torsoId;
        torso.GetComponent<Renderer>().material = torsoMaterials[torsoPos];

        legsPos = newUnit.legsId;
        var legMats = legs.GetComponent<Renderer>().sharedMaterials;
        legMats[1] = legMaterials[legsPos];

        legs.GetComponent<Renderer>().sharedMaterials = legMats;

    }
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

        if (armors[armorPos].GetComponent<Renderer>())
        {
            if (armors[armorPos].GetComponent<Renderer>().material.name.Contains("Cloth"))
            {
                armors[armorPos].GetComponent<Renderer>().material.color = factionManager.currentColor;
            }
        }

    }

    public void PreviousArmor()
    {
        armors[armorPos].SetActive(false);

        armorPos = PreviousPos(armorPos, armors.Count);
        armors[armorPos].SetActive(true);

        if (armors[armorPos].GetComponent<Renderer>())
        {
            if (armors[armorPos].GetComponent<Renderer>().material.name.Contains("Cloth"))
            {
                armors[armorPos].GetComponent<Renderer>().material.color = factionManager.currentColor;
            }
        }
    }
    public void NextTorso()
    {
        torsoPos = NextPos(torsoPos, torsoMaterials.Count);
        torso.GetComponent<Renderer>().material = torsoMaterials[torsoPos];

        if (torso.GetComponent<Renderer>().material.name.Contains("Cloth"))
        {
            torso.GetComponent<Renderer>().material.color = factionManager.currentColor;
        }
    }
    public void PreviousTorso()
    {
        torsoPos = PreviousPos(torsoPos, torsoMaterials.Count);
        torso.GetComponent<Renderer>().material = torsoMaterials[torsoPos];

        if (torso.GetComponent<Renderer>().material.name.Contains("Cloth"))
        {
            torso.GetComponent<Renderer>().material.color = factionManager.currentColor;
        }
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

    public void UpdateColor(Color newColor)
    {
        if (torso.GetComponent<Renderer>().material.name.Contains("Cloth"))
        {
            torso.GetComponent<Renderer>().material.color = newColor;
        }
        if (armors[armorPos].GetComponent<Renderer>())
        {
            if (armors[armorPos].GetComponent<Renderer>().material.name.Contains("Cloth"))
            {
                armors[armorPos].GetComponent<Renderer>().material.color = newColor;
            }
        }
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
