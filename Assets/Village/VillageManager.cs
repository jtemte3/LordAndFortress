using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public string villageId;
    public string currentFactionId = "0000";
    public Renderer bannerFlag;
    public GameObject gridPointIndicator;
    public Transform buildingParent;
    public Transform gridpointParent;
    public int gridSize;
    public int gridScale;
    private int halfGridSize;
    public float buildDistance;
    public List<GameObject> villageObjects = new();
    private List<GameObject> gridPoints = new();
    public LevelManager gameManager;
    public GameObject buildZone;

    //Dictionary<string, GridPoint> grid = new Dictionary<string, GridPoint>();
    public bool isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGridPoints();
        HideGridPoints();
        halfGridSize = (gridSize / 2) * gridScale;

        ChangeVillageFactionOwner(currentFactionId);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateGridPoints()
    {
        int halfGridSize = gridSize / 2;
        for (int x = -halfGridSize; x <= halfGridSize; x++)
        {
            for (int z = -halfGridSize; z <= halfGridSize; z++)
            {
                string coordinatesAsString = x + "," + z;

                GameObject pointObj = Instantiate(gridPointIndicator, new Vector3( this.transform.position.x + x * gridScale, this.transform.position.y, this.transform.position.z + z * gridScale), Quaternion.identity);
                pointObj.name = coordinatesAsString;
                pointObj.transform.parent = gridpointParent;
                gridPoints.Add(pointObj);
            }
        }
    }
    public void ShowGridPoints()
    {
        if (isHidden)
        {
            isHidden = false;
            //Debug.Log("showing gridpoints");
            buildZone.GetComponent<Renderer>().enabled = true;
            foreach (GameObject point in gridPoints)
            {
                point.GetComponent<Renderer>().enabled = true;
            }
        }
    }
    public void HideGridPoints()
    {
        if (!isHidden)
        {
            isHidden = true;
            //Debug.Log("hiding gridpoints");
            buildZone.GetComponent<Renderer>().enabled = false;
            foreach (GameObject point in gridPoints)
            {
                point.GetComponent<Renderer>().enabled = false;
            }
        }
    }
    public float GetBuildDistance()
    {
        return buildDistance;
    }

    public int GetGridScale()
    {
        return gridScale;
    }
    public int GetGridSize()
    {
        return gridSize;
    }
    public int GetHalfGridSize()
    {
        return halfGridSize;
    }

    public void AddBuilding(GameObject newBuilding)
    {
        villageObjects.Add(newBuilding);
        newBuilding.transform.parent = buildingParent;
    }

    public void RemoveBuilding(GameObject buildingToRemove)
    {
        villageObjects.Remove(buildingToRemove);
        Destroy(buildingToRemove);
    }

    public void ChangeVillageFactionOwner(string factionId)
    {
        FactionObject currentFaction = null;
        FactionObject newFaction = null;
        Color newFlagColor = Color.white;
        
        //find current and new faction
        foreach (FactionObject faction in gameManager.factions)
        {
            if (currentFactionId.Equals(faction.factionId))
            {
                currentFaction = faction;
            }
            if (factionId.Equals(faction.factionId))
            {
                newFaction = faction;
                newFlagColor = faction.factionColor;
            }
        }
        //change banner color
        bannerFlag.material.color = newFlagColor;

        //update all of the buildings in the village to match the new faction
        foreach (GameObject villageObject in villageObjects)
        {
            if (villageObject.GetComponent<VillageBuilding>() == true)
            {
                //remove house population from current faction to the new faction
                if (villageObject.GetComponent<VillageProductionScript>().productionType == VillageProductionScript.VillageProductionType.Population)
                {
                    currentFaction.currentPopulation -= villageObject.GetComponent<VillageProductionScript>().productionAmmount;
                    newFaction.currentPopulation += villageObject.GetComponent<VillageProductionScript>().productionAmmount;
                }
                //Update the owner to the new faction
                villageObject.GetComponent<VillageBuilding>().ChangeFactionOwner(newFaction);
            }
        }
        //switch over the factionId so that faction can build
        currentFactionId = factionId;
    }
}
