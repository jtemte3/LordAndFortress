using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksManager : MonoBehaviour
{
    public VillageManager villageManager;
    public GameObject unitSpawnPoint;
    public GameObject customUnitPrefab;
    // Start is called before the first frame update
    void Start()
    {
        villageManager = this.GetComponent<VillageBuilding>().GetVillage();
        villageManager.barracks = this.gameObject;
    }

    public void CreateUnitType(CustomUnitObject customUnit, Color factionColor, string factionId, int type, GameObject hero)
    {
        GameObject unit = Instantiate(customUnitPrefab, unitSpawnPoint.transform);
        unit.GetComponent<CustomUnitLoader>().LoadUnit(customUnit, factionColor);
        unit.GetComponent<FactionEntityData>().factionId = factionId;
        unit.GetComponent<FactionEntityData>().playerName = factionId + "-Unit-" + type;
        unit.gameObject.name = factionId + "-Unit-" + type;
        unit.transform.parent = null;

        unit.AddComponent<CustomTroopMovement>();
        unit.GetComponent<CustomTroopMovement>().hero = hero;
        unit.GetComponent<CustomTroopMovement>().speed = customUnit.unitSpeed;
    }
}
