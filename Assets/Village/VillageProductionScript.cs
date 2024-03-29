using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageProductionScript : MonoBehaviour
{
    public enum VillageProductionType {Wood, Food, Stone, Population, Gold}
    public VillageProductionType productionType;
    public int productionAmmount;
    public float productionRate;
    private float nextProduction;
    private VillageBuilding villageBuilding;

    private void Start()
    {
        villageBuilding = this.gameObject.GetComponent<VillageBuilding>();
        if (productionType != VillageProductionType.Population)
        {
            nextProduction = Time.time + productionRate;
        }
        else
        {
            villageBuilding.GetVillage().GetCurrentFaction().currentPopulation += productionAmmount;
        }
    }
    private void Update()
    {
        if (productionType != VillageProductionType.Population)
        {
            if (Time.time >= nextProduction)
            {
                nextProduction = Time.time + productionRate;
                if (productionType == VillageProductionType.Wood)
                {
                    villageBuilding.GetVillage().GetCurrentFaction().currentWood += productionAmmount;
                }
                if (productionType == VillageProductionType.Food)
                {
                    villageBuilding.GetVillage().GetCurrentFaction().currentFood += productionAmmount;
                }
                if (productionType == VillageProductionType.Stone)
                {
                    villageBuilding.GetVillage().GetCurrentFaction().currentStone += productionAmmount;
                }

            }
        }
    }

    private void OnDestroy()
    {
        if (productionType == VillageProductionType.Population)
        {
            villageBuilding.GetVillage().GetCurrentFaction().currentPopulation -= productionAmmount;
            if (villageBuilding.GetVillage().GetCurrentFaction().currentPopulation < 0)
            {
                villageBuilding.GetVillage().GetCurrentFaction().currentPopulation = 0;
            }
        }
    }
}
