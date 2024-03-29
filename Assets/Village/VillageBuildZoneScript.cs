using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuildZoneScript : MonoBehaviour
{
    public VillageManager villageManager;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<BuildSystemController>())
        {
            BuildSystemController builder = other.gameObject.GetComponent<BuildSystemController>();
            if (builder.currentFaction.factionId == villageManager.currentFactionId)
            {
                builder.currentVillage = villageManager;
                builder.canBuild = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BuildSystemController>())
        {
            BuildSystemController builder = other.gameObject.GetComponent<BuildSystemController>();
            if (builder.currentFaction.factionId == villageManager.currentFactionId)
            {
                builder.currentVillage = null;
                builder.canBuild = false;
                builder.isBuilding = false;
                if (!villageManager.isHidden)
                {
                    villageManager.HideGridPoints();
                }
            }
        }
    }

    public void AdjustBoundryZone(int gridSize, int gridScale)
    {
        int villageBounds = (gridSize * gridScale) + gridScale;
        transform.localScale = new Vector3(villageBounds, 5, villageBounds);
    }
}
