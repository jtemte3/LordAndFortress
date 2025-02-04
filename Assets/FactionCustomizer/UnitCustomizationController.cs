using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCustomizationController : MonoBehaviour
{
    public FactionManager factionManager;
    public List<GameObject> helmets = new();
    public List<Material> baseArmorMaterials = new();
    public List<GameObject> armors = new();
    public List<Material> legMaterials = new();
    public List<GameObject> weapons = new();
    public List<GameObject> shields = new();
    public GameObject legs;

    int helmetPos = 0;
    int baseArmorMatPos = 0;
    int armorPos = 0;
    int weaponPos = 0;
    int shieldPos = 0;
    int legMatPos = 0;

    private void Start()
    {
        for (int i = 0; i < baseArmorMaterials.Count; i++)
        {
            if (baseArmorMaterials[i].name.Contains("Cloth"))
            {
                Material tempMaterial = baseArmorMaterials[i];

                baseArmorMaterials[i] = new Material(tempMaterial);
            }
        }

        foreach (GameObject armor in armors)
        {
            var armorMats = armor.GetComponent<Renderer>().sharedMaterials;
            for (int i = 0; i < armorMats.Length; i++)
            {
                if (armorMats[i].name.Contains("Tabbard"))
                {
                    Material tempMaterial = armorMats[i];
                    armorMats[i] = new Material(tempMaterial);
                }
            }
            armor.GetComponent<Renderer>().sharedMaterials = armorMats;
        }

        foreach (GameObject armor in armors)
        {
            var armorMats = armor.GetComponent<Renderer>().sharedMaterials;
            for (int i = 0; i < armorMats.Length; i++)
            {
                if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
                {
                    armorMats[i] = baseArmorMaterials[baseArmorMatPos];
                }
                if (armorMats[i].name.Contains("ChainMail"))
                {
                    armorMats[i] = baseArmorMaterials[baseArmorMatPos];
                }
            }
            armor.GetComponent<Renderer>().sharedMaterials = armorMats;
        }
    }

    public CustomUnitObject ExportUnit()
    {
        CustomUnitObject currentUnit = new();
        currentUnit.helmetId = helmetPos;
        currentUnit.baseArmorId = baseArmorMatPos;
        currentUnit.armorId = armorPos;
        currentUnit.legsId = legMatPos;
        currentUnit.weaponId = weaponPos;
        currentUnit.shieldId = shieldPos;
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

        foreach (GameObject obj in weapons)
        {
            obj.SetActive(false);
        }

        weaponPos = newUnit.weaponId;
        weapons[weaponPos].SetActive(true);

        foreach (GameObject obj in shields)
        {
            obj.SetActive(false);
        }

        shieldPos = newUnit.shieldId;
        shields[shieldPos].SetActive(true);

        foreach (GameObject obj in armors)
        {
            obj.SetActive(false);
        }
        armorPos = newUnit.armorId;
        armors[armorPos].SetActive(true);

        baseArmorMatPos = newUnit.baseArmorId;
        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
        }
        armors[armorPos].GetComponent<Renderer>().sharedMaterials = armorMats;

        legMatPos = newUnit.legsId;
        var legMats = legs.GetComponent<Renderer>().sharedMaterials;
        legMats[1] = legMaterials[legMatPos];

        legs.GetComponent<Renderer>().sharedMaterials = legMats;
        legs.SetActive(true);

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

    public void NextWeapon()
    {
        weapons[weaponPos].SetActive(false);

        weaponPos = NextPos(weaponPos, weapons.Count);
        weapons[weaponPos].SetActive(true);
    }

    public void PreviousWeapon()
    {
        weapons[weaponPos].SetActive(false);

        weaponPos = PreviousPos(weaponPos, weapons.Count);
        weapons[weaponPos].SetActive(true);
    }

    public void NextShield()
    {
        shields[shieldPos].SetActive(false);

        shieldPos = NextPos(shieldPos, shields.Count);
        shields[shieldPos].SetActive(true);
    }

    public void PreviousShield()
    {
        shields[shieldPos].SetActive(false);

        shieldPos = PreviousPos(shieldPos, shields.Count);
        shields[shieldPos].SetActive(true);
    }

    public void NextArmor()
    {
        armors[armorPos].SetActive(false);

        armorPos = NextPos(armorPos, armors.Count);
        armors[armorPos].SetActive(true);

        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = factionManager.currentColor;
            }
        }
        armors[armorPos].GetComponent<Renderer>().sharedMaterials = armorMats;

    }

    public void PreviousArmor()
    {
        armors[armorPos].SetActive(false);

        armorPos = PreviousPos(armorPos, armors.Count);
        armors[armorPos].SetActive(true);

        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = factionManager.currentColor;
            }
        }
        armors[armorPos].GetComponent<Renderer>().sharedMaterials = armorMats;
    }
    public void NextTorso()
    {
        baseArmorMatPos = NextPos(baseArmorMatPos, baseArmorMaterials.Count);
        //torso.GetComponent<Renderer>().material = torsoMaterials[armorMatPos];

        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = factionManager.currentColor;
            }
        }

        armors[armorPos].GetComponent<Renderer>().sharedMaterials = armorMats;
    }
    public void PreviousTorso()
    {
        baseArmorMatPos = PreviousPos(baseArmorMatPos, baseArmorMaterials.Count);
        //torso.GetComponent<Renderer>().material = torsoMaterials[armorMatPos];

        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[baseArmorMatPos];
            }
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = factionManager.currentColor;
            }
        }
    }
    public void NextLegs()
    {
        legMatPos = NextPos(legMatPos, legMaterials.Count);

        var mats = legs.GetComponent<Renderer>().sharedMaterials;
        mats[1] = legMaterials[legMatPos];
        legs.GetComponent<Renderer>().sharedMaterials = mats;
    }
    public void PreviousLegs()
    {
        legMatPos = PreviousPos(legMatPos, legMaterials.Count);

        var mats = legs.GetComponent<Renderer>().sharedMaterials;
        mats[1] = legMaterials[legMatPos];
        legs.GetComponent<Renderer>().sharedMaterials = mats;
    }

    public void UpdateColor(Color newColor)
    {
        var armorMats = armors[armorPos].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = newColor;
            }
        }
        armors[armorPos].GetComponent<Renderer>().sharedMaterials = armorMats;
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
