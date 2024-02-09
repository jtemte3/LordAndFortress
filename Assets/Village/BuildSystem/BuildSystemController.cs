using System;
using UnityEngine;

public class BuildSystemController : MonoBehaviour
{
    public LevelManager gameManager;
    public GameObject BuildMenuPanel;
    private BuildableObject currentObject = null;
    private Vector3 currentPosition;
    private Vector3 currentRotation;
    private Transform currentPreview = null;
    public Transform cam;
    private RaycastHit hit;
    public LayerMask layer;
    public float previewDistance;
    public float rotationAmount= 45;
    private bool isBuilding;
    private bool canBuild;
    private bool showBuildMenu;
    private VillageManager currentVillage;


    // Start is called before the first frame update
    void Start()
    {
        showBuildMenu = false;
    }

    private void Update()
    {
        //Determine if player is in range of a village and can initiate building
        ValidateVillageRange();

        //When crossing into the build distance threshold for a village, assign the village to the active village
        if (canBuild && currentVillage == null)
        {
            currentVillage = FindClosestVillage();
        }

        //Listen for the Tab button to initiate the building mode
        if (Input.GetKeyDown(KeyCode.Tab) && canBuild)
        {
            //if building mode is already on, then turn it off
            if (isBuilding)
            {
                isBuilding = false;
                currentVillage.HideGridPoints();
                if (currentPreview != null)
                {
                    currentObject = null;
                    RemovePreviewObject();
                }
            }
            //if building mode is not on, enable it
            else
            {
                isBuilding = true;
                currentVillage.ShowGridPoints();               
            }
        }
        //Leaving the build distance threshold for a village, turn everything off and reset values
        if (!canBuild)
        {
            if (currentVillage != null)
            {
                currentVillage.HideGridPoints();
                currentVillage = null;
                isBuilding = false;
            }
            if (currentPreview != null)
            {
                currentObject = null;
                RemovePreviewObject();
            }
        }

        if (canBuild && isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (showBuildMenu)
                {
                    showBuildMenu = false;
                }
                else
                {
                    showBuildMenu = true;
                    if (currentPreview != null)
                    {
                        currentObject = null;
                        RemovePreviewObject();
                    }
                }
            }
        }

        if (showBuildMenu)
        {
            BuildMenuPanel.SetActive(true);
            //Unlock the Cursor
            Cursor.lockState = CursorLockMode.None;
            //Set Cursor to be visible
            Cursor.visible = true;
        }
        else
        {
            BuildMenuPanel.SetActive(false);
            //Lock the Cursor
            Cursor.lockState = CursorLockMode.Locked;
            //Set Cursor to not be visible
            Cursor.visible = false;
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
                        currentObject = null;
                        RemovePreviewObject();
                    }
                    if (input <= 9 && input >= 1)
                    {
                        currentObject = gameManager.objects[input - 1];
                        ChangePreviewObject();
                    }
                }
                catch
                {
                    input = -1;
                }
            }
        }

        if (canBuild && isBuilding && currentPreview != null)
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, previewDistance, layer))
            {
                if (currentPreview != null)
                {
                    movePreview(currentVillage);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentRotation -= new Vector3(0, rotationAmount, 0);
                currentPreview.localEulerAngles = currentRotation;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentRotation += new Vector3(0, rotationAmount, 0);
                currentPreview.localEulerAngles = currentRotation;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild && isBuilding && currentPreview != null)
            {
                AttemptBuild(currentVillage);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (canBuild && isBuilding && currentPreview != null)
            {
                AttemptDelete(currentVillage);
            }
        }
    }

    public void ChangeBuildObject(string objectId)
    {
        foreach( BuildableObject buildObject in gameManager.objects)
        {
            if (buildObject.buildingId.Equals(objectId))
            {
                currentObject = buildObject;
                ChangePreviewObject();
                showBuildMenu = false;
                BuildMenuPanel.SetActive(false);
                break;
            }
        }
    }

    void ChangePreviewObject()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }
        GameObject previewObj = Instantiate(currentObject.preview, currentPosition, Quaternion.Euler(currentRotation)) as GameObject;
        currentPreview = previewObj.transform;
    }

    void RemovePreviewObject()
    {
        Destroy(currentPreview.gameObject);
        currentPreview = null;
    }

    void movePreview(VillageManager village)
    {
        currentPosition = SnapToGrid(currentObject, village);
        currentPreview.position = currentPosition;
    }

    void AttemptBuild(VillageManager village)
    {
        PreviewScript previewObj = currentPreview.GetComponent<PreviewScript>();
        if (previewObj.canBuild)
        {
            GameObject newObject = Instantiate(currentObject.prefab, currentPosition, Quaternion.Euler(currentRotation));
            village.AddBuilding(newObject);
        }
    }

    void AttemptDelete(VillageManager village)
    {
        PreviewScript previewObj = currentPreview.GetComponent<PreviewScript>();
        if (!previewObj.canBuild && previewObj.colList.Count > 0)
        {
            GameObject objectToDelete = previewObj.colList[0].gameObject;
            if (objectToDelete.name != "VillageBanner")
            {
                village.RemoveBuilding(objectToDelete);
                previewObj.colList.RemoveAt(0);
            }
        }
    }

    private Vector3 SnapToGrid(BuildableObject obj, VillageManager village)
    {
        if (!hit.point.Equals(null))
        {
            Vector3 snapped;

            Vector3 limitedPoint = new();
            limitedPoint.x = EnforceGridLimits(hit.point.x, village.transform.position.x, village.GetHalfGridSize());
            limitedPoint.z = EnforceGridLimits(hit.point.z, village.transform.position.z, village.GetHalfGridSize());

            snapped.x = village.GetGridScale() * (Mathf.Round(limitedPoint.x / village.GetGridScale())) + obj.buildingHorizontalOffset;
            snapped.y = village.transform.position.y + obj.buildHeightOffset;
            snapped.z = village.GetGridScale() * (Mathf.Round(limitedPoint.z / village.GetGridScale())) + obj.buildingForwardOffset;

            return snapped;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private float EnforceGridLimits(float point, float villageCenterPoint, int villageHalfSize)
    {
        float pointDist = point - villageCenterPoint;

        float adjustedPoint = point;

        if (pointDist >= 0)
        {
            if (pointDist > villageHalfSize)
            {
                adjustedPoint = villageCenterPoint + villageHalfSize;
            }
        }
        else
        {
            if (pointDist < -villageHalfSize)
            {
                adjustedPoint = villageCenterPoint - villageHalfSize;
            }
        }

        return adjustedPoint;
    }

    private void ValidateVillageRange()
    {
        int validVillages = 0;

        foreach (VillageManager villageManager in gameManager.villages)
        {
            float dist = Vector3.Distance(this.transform.position, villageManager.transform.position);
            if (dist <= villageManager.GetBuildDistance())
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

    private VillageManager FindClosestVillage()
    {
        VillageManager closestVillage = gameManager.villages[0];

        float closestVillageDist = Vector3.Distance(this.transform.position, closestVillage.transform.position);

        foreach (VillageManager villageManager in gameManager.villages)
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
