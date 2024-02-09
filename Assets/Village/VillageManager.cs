using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public string villageId;
    public GameObject gridPointIndicator;
    public Transform buildingParent;
    public Transform gridpointParent;
    public int gridSize;
    public int gridScale;
    private int halfGridSize;
    public float buildDistance;
    public List<GameObject> villageObjects = new();
    private List<GameObject> gridPoints = new();

    //Dictionary<string, GridPoint> grid = new Dictionary<string, GridPoint>();
    private bool isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGridPoints();
        HideGridPoints();
        halfGridSize = (gridSize / 2) * gridScale;
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
            Debug.Log("showing gridpoints");
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
            Debug.Log("hiding gridpoints");
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
}
