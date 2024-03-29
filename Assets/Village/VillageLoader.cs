using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageLoader : MonoBehaviour
{
    public VillageManager village;
    public string fileName;

    public float timer;
    // Start is called before the first frame update
    void Update()
    {
        if (!village.loadFromFile)
        {
            this.gameObject.GetComponent<VillageLoader>().enabled = false;
        }

        if (village.GetCurrentFaction() != null && Time.time <= timer && village.loadFromFile)
        {
            VillageDataObject newVillage = new FileUtils().LoadVillageFromFile(fileName);

            village.DeleteGridPoints();
            village.SetGridSize(newVillage.villageSize);
            village.GenerateGridPoints(newVillage.villageSize);
            village.HideGridPoints();
            village.buildZone.GetComponent<VillageBuildZoneScript>().AdjustBoundryZone(village.GetGridSize(), village.GetGridScale());

            foreach (BuildingDataObject newBuilding in newVillage.buildings)
            {
                village.AddBuilding(newBuilding);
            }

            this.gameObject.GetComponent<VillageLoader>().enabled = false;

        }
    }

}
