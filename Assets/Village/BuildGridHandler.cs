using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridHandler : MonoBehaviour
{
    public GameObject testObject;
    public int gridSize;
    public int gridScale;
    public Vector3 buildOffset;
    public float buildDistance;
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> gridPoints = new List<GameObject>();

    //Dictionary<string, GridPoint> grid = new Dictionary<string, GridPoint>();
    private bool isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGridPoints();
        HideGridPoints();
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

                GameObject pointObj = Instantiate(testObject, new Vector3( this.transform.position.x + x * gridScale, this.transform.position.y, this.transform.position.z + z * gridScale), Quaternion.identity);
                pointObj.name = coordinatesAsString;
                pointObj.transform.parent = this.transform;
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

    public float getBuildDistance()
    {
        return buildDistance;
    }
    public Vector3 getBuildOffset()
    {
        return buildOffset;
    }
    public int getGridScale()
    {
        return gridScale;
    }
}
