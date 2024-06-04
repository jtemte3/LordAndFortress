using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnitLoader : MonoBehaviour
{
    public List<GameObject> helmets = new();
    public List<Material> baseArmorMaterials = new();
    public List<GameObject> armors = new();
    public List<Material> legMaterials = new();
    public List<GameObject> weapons = new();
    public List<GameObject> shields = new();
    public GameObject legs;

    public void LoadUnit(CustomUnitObject newUnit, Color factionColor)
    {
        for (int i = 0; i < helmets.Count; i++)
        {
            if (newUnit.helmetId.Equals(i))
            {
                helmets[i].SetActive(true);
            }
            else
            {
                Destroy(helmets[i]);
            }
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (newUnit.weaponId.Equals(i))
            {
                weapons[i].SetActive(true);
            }
            else
            {
                Destroy(weapons[i]);
            }
        }

        for (int i = 0; i < shields.Count; i++)
        {
            if (newUnit.shieldId.Equals(i))
            {
                shields[i].SetActive(true);
            }
            else
            {
                Destroy(shields[i]);
            }
        }

        for (int i = 0; i < armors.Count; i++)
        {
            if (newUnit.armorId.Equals(i))
            {
                armors[i].SetActive(true);
            }
            else
            {
                Destroy(armors[i]);
            }
        }

        var armorMats = armors[newUnit.armorId].GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < armorMats.Length; i++)
        {
            if (armorMats[i].name.Contains("Cloth") && !armorMats[i].name.Contains("Tabbard"))
            {
                armorMats[i] = baseArmorMaterials[newUnit.baseArmorId];
            }
            if (armorMats[i].name.Contains("ChainMail"))
            {
                armorMats[i] = baseArmorMaterials[newUnit.baseArmorId];
            }
            if (armorMats[i].name.Contains("Cloth"))
            {
                armorMats[i].color = factionColor;
            }
        }
        armors[newUnit.armorId].GetComponent<Renderer>().sharedMaterials = armorMats;

        var legMats = legs.GetComponent<Renderer>().sharedMaterials;
        legMats[1] = legMaterials[newUnit.legsId];

        legs.GetComponent<Renderer>().sharedMaterials = legMats;
        legs.SetActive(true);

    }

}
