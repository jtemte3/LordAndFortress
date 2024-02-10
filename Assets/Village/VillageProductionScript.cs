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
            villageBuilding.faction.currentPopulation += productionAmmount;
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
                    villageBuilding.faction.currentWood += productionAmmount;
                }
                if (productionType == VillageProductionType.Food)
                {
                    villageBuilding.faction.currentFood += productionAmmount;
                }
                if (productionType == VillageProductionType.Stone)
                {
                    villageBuilding.faction.currentStone += productionAmmount;
                }

            }
        }
    }

    private void OnDestroy()
    {
        if (productionType == VillageProductionType.Population)
        {
            villageBuilding.faction.currentPopulation -= productionAmmount;
            if (villageBuilding.faction.currentPopulation < 0)
            {
                villageBuilding.faction.currentPopulation = 0;
            }
        }
    }
}
