using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public string villageId;
    public string currentFactionId = "0000";
    private FactionObject currentFaction;
    public Renderer bannerFlag;
    public GameObject gridPointIndicator;
    public Transform buildingParent;
    public Transform gridpointParent;
    public int gridSize;
    public const int gridScale = 4;
    public Vector3 villageOffset;
    public Vector3 villageOffsetResult;
    private int halfGridSize;
    public List<GameObject> villageObjects = new();
    public Dictionary<Vector3, GameObject> villageMap = new();
    private List<GameObject> gridPoints = new();
    public LevelManager gameManager;
    public GameObject buildZone;
    public bool showBuildZone = false;
    public bool canBuild = false;
    public bool loadFromFile;
    public GameObject barracks;
    public GameObject barracksPanel;

    //Dictionary<string, GridPoint> grid = new Dictionary<string, GridPoint>();
    public bool isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        currentFaction = GetFactionDetails(currentFactionId);

        if (!loadFromFile)
        {
            buildZone.GetComponent<VillageBuildZoneScript>().AdjustBoundryZone(gridSize, gridScale);
            GenerateGridPoints(gridSize);
            HideGridPoints();
        }

        halfGridSize = (gridSize / 2) * gridScale;

        ChangeVillageFlagColor(currentFactionId);
        ChangeVillageFactionOwner(currentFactionId);

        float roundedX = gridScale * Mathf.Round(transform.position.x / gridScale);
        float roundedZ = gridScale * Mathf.Round(transform.position.z / gridScale);
        Vector3 villageRoundedPos = new(roundedX, transform.position.y, roundedZ);

        villageOffset = transform.position - villageRoundedPos;

        villageOffsetResult.x = transform.position.x - villageOffset.x;
        villageOffsetResult.y = transform.position.y - villageOffset.y;
        villageOffsetResult.z = transform.position.z - villageOffset.z;

        villageMap.Add(new Vector3(0, 0, 0), transform.Find("VillageBanner").gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (canBuild && barracks != null)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (barracksPanel.activeSelf != true)
                {
                    gameManager.showCursor = true;
                    barracksPanel.GetComponent<BarracksMenuHandler>().ClearListeners();
                    barracksPanel.GetComponent<BarracksMenuHandler>().Setup(this);
                    barracksPanel.SetActive(true);
                }
                else
                {
                    barracksPanel.GetComponent<BarracksMenuHandler>().ClearListeners();
                    gameManager.showCursor = false;
                    barracksPanel.SetActive(false);
                }
            }
        }
        
    }

    public void GenerateGridPoints(int newGridSize)
    {
        int halfGridSize = newGridSize / 2;
        for (int x = -halfGridSize; x <= halfGridSize; x++)
        {
            for (int z = -halfGridSize; z <= halfGridSize; z++)
            {
                string coordinatesAsString = x + "," + z;

                GameObject pointObj = Instantiate(gridPointIndicator, new Vector3(this.transform.position.x + x * gridScale, this.transform.position.y, this.transform.position.z + z * gridScale), Quaternion.identity);
                pointObj.name = coordinatesAsString;
                pointObj.transform.parent = gridpointParent;
                gridPoints.Add(pointObj);
            }
        }
    }

    public void DeleteGridPoints()
    {
        foreach (GameObject point in gridPoints)
        {
            //gridPoints.Remove(point);
            Destroy(point);
        }
    }
    public void ShowGridPoints()
    {
        if (isHidden)
        {
            isHidden = false;
            //Debug.Log("showing gridpoints");
            if (showBuildZone)
            {
                buildZone.GetComponent<Renderer>().enabled = true;
            }
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
            if (showBuildZone)
            {
                buildZone.GetComponent<Renderer>().enabled = false;
            }
            foreach (GameObject point in gridPoints)
            {
                point.GetComponent<Renderer>().enabled = false;
            }
        }
    }
    public int GetGridScale()
    {
        return gridScale;
    }
    public int GetGridSize()
    {
        return gridSize;
    }
    public void SetGridSize(int newSize)
    {
        gridSize = newSize;
    }
    public int GetHalfGridSize()
    {
        return halfGridSize;
    }
    public void SetCanBuild(bool state)
    {
        canBuild = state;
    }
    public bool GetCanBuild()
    {
        return canBuild;
    }
    public FactionObject GetCurrentFaction()
    {
        return currentFaction;
    }

    public void AddBuilding(GameObject newBuilding)
    {
        Vector3 buildingCoordinate = transform.position - newBuilding.transform.position;

        newBuilding.GetComponent<VillageBuilding>().SetVillage(this);
        newBuilding.GetComponent<VillageBuilding>().SetCoordinate(buildingCoordinate);

        villageObjects.Add(newBuilding);
        villageMap.Add(buildingCoordinate, newBuilding);

        newBuilding.transform.parent = buildingParent;
    }

    public void AddBuilding(BuildingDataObject newBuildingData)
    {
        BuildableObject newBuildingObject = new();

        Vector3 newBuildingPosition = transform.position + (newBuildingData.GetCoordinate() * -1);

        foreach (BuildableObject buildObject in gameManager.objects)
        {
            if (buildObject.buildingId.Equals(newBuildingData.buildingId))
            {
                newBuildingObject = buildObject;
                break;
            }
        }

        //newBuildingPosition.y += newBuildingObject.buildHeightOffset;

        GameObject newBuilding = Instantiate(newBuildingObject.prefab, newBuildingPosition, Quaternion.Euler(newBuildingData.GetRotation().eulerAngles));

        newBuilding.GetComponent<VillageBuilding>().SetVillage(this);
        newBuilding.GetComponent<VillageBuilding>().SetCoordinate(newBuildingData.GetCoordinate());

        villageObjects.Add(newBuilding);
        villageMap.Add(newBuildingData.GetCoordinate(), newBuilding);

        newBuilding.transform.parent = buildingParent;
    }

    public void RemoveBuilding(GameObject buildingToRemove)
    {
        Vector3 buildingCoordinate = buildingToRemove.GetComponent<VillageBuilding>().GetCoordinate();

        villageObjects.Remove(buildingToRemove);
        villageMap.Remove(buildingCoordinate);

        Destroy(buildingToRemove);
    }

    public void ChangeVillageFlagColor(string factionId)
    {
        Color newFlagColor = Color.white;

        newFlagColor = GetFactionDetails(factionId).factionColor;
        //change banner color
        bannerFlag.material.color = newFlagColor;
    }

    public void ChangeVillageFlagColor(Color factionColor)
    {
        bannerFlag.material.color = factionColor;
    }

    public void ChangeVillageFactionOwner(string factionId)
    {
        //Find the new faction object
        FactionObject newFaction = GetFactionDetails(factionId);

        //update all of the buildings in the village to match the new faction
        foreach (GameObject villageObject in villageObjects)
        {
            if (villageObject.GetComponent<VillageBuilding>() != null)
            {
                if (villageObject.GetComponent<VillageProductionScript>() != null)
                {
                    //remove house population from current faction to the new faction
                    if (villageObject.GetComponent<VillageProductionScript>().productionType == VillageProductionScript.VillageProductionType.Population)
                    {
                        currentFaction.currentPopulation -= villageObject.GetComponent<VillageProductionScript>().productionAmmount;
                        newFaction.currentPopulation += villageObject.GetComponent<VillageProductionScript>().productionAmmount;
                    }
                }
            }
        }
        //switch over the factionId so that faction can build
        currentFactionId = factionId;
        currentFaction = newFaction;
    }

    private FactionObject GetFactionDetails(string factionId)
    {
        FactionObject faction = new();

        foreach (FactionObject factionObj in gameManager.factions)
        {
            if (factionId.Equals(factionObj.factionId))
            {
                faction = factionObj;
                break;
            }
        }

        return faction;
    }

    public void SpawnUnit(int type)
    {
        if (barracks != null)
        {
            CustomUnitObject customUnit = currentFaction.customFactionObject.customUnits[type];
            barracks.GetComponent<BarracksManager>().CreateUnitType(customUnit, currentFaction.factionColor, currentFaction.factionId, type);
        }
    }
}
