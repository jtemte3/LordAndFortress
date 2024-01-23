using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystemController : MonoBehaviour
{
    public List<BuildObject> objects = new List<BuildObject>();
    private BuildObject currentObject;
    private Vector3 currentPosition;
    private Vector3 currentRotation;
    private Transform currentPreview;
    public Transform cam;
    private RaycastHit hit;
    public LayerMask layer;
    public float previewDistance;
    public bool isBuilding;
    public bool canBuild;
    private List<BuildGridHandler> villages = new List<BuildGridHandler>();
    public BuildGridHandler currentVillage;


    // Start is called before the first frame update
    void Start()
    {
        currentObject = objects[0];
        changeCurrentObject();

        currentPreview.gameObject.SetActive(isBuilding);
        villages.AddRange(FindObjectsOfType<BuildGridHandler>());
    }

    private void Update()
    {
        ValidateVillageRange();

        if (canBuild && currentVillage == null)
        {
            currentVillage = FindClosestVillage();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && canBuild)
        {
            if (isBuilding)
            {
                isBuilding = false;
                currentVillage.HideGridPoints();
                currentPreview.gameObject.SetActive(false);
            }
            else
            {
                isBuilding = true;
                currentVillage.ShowGridPoints();
                currentPreview.GetComponent<PreviewScript>().colList.Clear();
                currentPreview.gameObject.SetActive(true);
            }
        }

        if (!canBuild)
        {
            if (currentVillage != null)
            {
                currentVillage.HideGridPoints();
                currentVillage = null;
                isBuilding = false;
                currentPreview.gameObject.SetActive(false);
            }
        }

        if (Input.anyKeyDown)
        {
            int input = -1;
            if (isBuilding)
            {
                try
                {
                    int.TryParse(Input.inputString, out input);
                    if (Input.inputString == "-")
                    {
                        currentObject = objects[0];
                        changeCurrentObject();

                    }
                    if (input <= 9 && input >= 1)
                    {
                        currentObject = objects[input - 1];
                        changeCurrentObject();
                    }
                }
                catch
                {
                    input = -1;
                }
            }

        }

        if (canBuild && isBuilding)
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, previewDistance, layer))
            {
                Debug.DrawRay(cam.position, hit.point, Color.red);
                if (hit.transform != this.transform)
                {
                    showPreview(currentVillage);
                    if (Input.GetMouseButtonDown(0))
                    {
                        attemptBuild(currentVillage);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        attemptDelete(currentVillage);
                    }

                }
            }
        }
    }

    void changeCurrentObject()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }
        GameObject previewObj = Instantiate(currentObject.preview, currentPosition, Quaternion.Euler(currentRotation)) as GameObject;
        currentPreview = previewObj.transform;
    }

    void showPreview(BuildGridHandler village)
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotation -= new Vector3(0, 45, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRotation += new Vector3(0, 45, 0);
        }

        if (validatePreviewPosition(snapToGrid(currentObject, village)))
        {
            currentPosition = snapToGrid(currentObject, village);
            currentPreview.position = currentPosition;
        }
        currentPreview.localEulerAngles = currentRotation;
    }

    private bool validatePreviewPosition(Vector3 currentPosition)
    {
        int halfGridSize = (currentVillage.gridSize / 2) * currentVillage.gridScale;
        Vector3 villageLocation = currentVillage.transform.position;
        
        float pointDistX = MathF.Abs(villageLocation.x - currentPosition.x);
        float pointDistZ = MathF.Abs(villageLocation.z - currentPosition.z);

        if (pointDistX <= halfGridSize && pointDistZ <= halfGridSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void attemptBuild(BuildGridHandler village)
    {
        PreviewScript previewObj = currentPreview.GetComponent<PreviewScript>();
        if (previewObj.canBuild)
        {
            GameObject newObject = Instantiate(currentObject.prefab, currentPosition, Quaternion.Euler(currentRotation));
            village.objects.Add(newObject);
        }
    }

    void attemptDelete(BuildGridHandler village)
    {
        PreviewScript previewObj = currentPreview.GetComponent<PreviewScript>();
        if (!previewObj.canBuild && previewObj.colList.Count > 0)
        {
            GameObject objectToDelete = previewObj.colList[0].gameObject;
            village.objects.Remove(objectToDelete);
            previewObj.colList.RemoveAt(0);
            Destroy(objectToDelete);
        }
    }

    private Vector3 snapToGrid(BuildObject obj, BuildGridHandler village)
    {
        if (!hit.point.Equals(null))
        {
            Vector3 snapped;

            snapped.x = village.getGridScale() * (Mathf.Round(hit.point.x / village.getGridScale())) + obj.buildingHorizontalOffset + village.getBuildOffset().x;
            snapped.y = village.transform.position.y + obj.buildHeightOffset + village.getBuildOffset().y;
            snapped.z = village.getGridScale() * (Mathf.Round(hit.point.z / village.getGridScale())) + obj.buildingForwardOffset + village.getBuildOffset().z;

            return snapped;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void ValidateVillageRange()
    {
        int validVillages = 0;

        foreach (BuildGridHandler villageManager in villages)
        {
            float dist = Vector3.Distance(this.transform.position, villageManager.transform.position);
            if (dist <= villageManager.getBuildDistance())
            {
                validVillages++;
            }
        }

        if (validVillages >= 1)
        {
            canBuild = true;
        }
        else
        {
            canBuild = false;
        }
    }

    private BuildGridHandler FindClosestVillage()
    {
        BuildGridHandler closestVillage = villages[0];

        float closestVillageDist = Vector3.Distance(this.transform.position, closestVillage.transform.position);

        foreach (BuildGridHandler villageManager in villages)
        {
            float dist = Vector3.Distance(this.transform.position, villageManager.transform.position);
            if (dist <= closestVillageDist)
            {
                closestVillage = villageManager;
            }
        }

        return closestVillage;
    }

}
