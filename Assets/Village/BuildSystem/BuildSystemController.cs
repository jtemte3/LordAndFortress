using System;
using UnityEngine;

public class BuildSystemController : MonoBehaviour
{
    public LevelManager gameManager;
    public FactionEntityData player;
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
    public bool isBuilding;
    public bool canBuild;
    private bool showBuildMenu;
    public VillageManager currentVillage;
    public FactionObject currentFaction;
    public bool changeFaction = false;

    // Start is called before the first frame update
    void Start()
    {
        showBuildMenu = false;
        foreach(FactionObject faction in gameManager.factions)
        {
            if (faction.factionId == player.factionId)
            {
                currentFaction = faction;
            }
        }
    }

    private void Update()
    {
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
                if (showBuildMenu)
                {
                    showBuildMenu = false;
                    BuildMenuPanel.SetActive(false);
                    gameManager.showCursor = false;
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
                    BuildMenuPanel.SetActive(false);
                    gameManager.showCursor = false;
                }
                else
                {
                    showBuildMenu = true;
                    if (currentPreview != null)
                    {
                        currentObject = null;
                        RemovePreviewObject();
                    }
                    BuildMenuPanel.SetActive(true);
                    gameManager.showCursor = true;
                }
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

        //placement mode
        if (canBuild && isBuilding && currentPreview != null)
        {
            currentPreview.GetComponent<PreviewScript>().SetHasResources(CheckForResources());

            if (Physics.Raycast(cam.position, cam.forward, out hit, previewDistance, layer))
            {
                if (currentPreview != null)
                {
                    MovePreview(currentVillage);
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

        //Build current object
        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild && isBuilding && currentPreview != null)
            {
                AttemptBuild(currentVillage);
            }
        }

        //Delete item preview is colliding with
        if (Input.GetMouseButtonDown(1))
        {
            if (canBuild && isBuilding && currentPreview != null)
            {
                AttemptDelete(currentVillage);
            }
        }

        //Just for testing.. allows the dev to change factions
        if (changeFaction)
        {
            foreach (FactionObject faction in gameManager.factions)
            {
                if (faction.factionId == player.factionId)
                {
                    currentFaction = faction;
                }
            }
            changeFaction = false;
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
                gameManager.showCursor = false;
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

    void MovePreview(VillageManager village)
    {
        currentPosition = SnapToGrid(currentObject, village);
        currentPreview.position = currentPosition;
    }

    bool CheckForResources()
    {
        if (currentFaction.currentWood >= currentObject.woodCost && currentFaction.currentStone >= currentObject.stoneCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AttemptBuild(VillageManager village)
    {
        PreviewScript previewObj = currentPreview.GetComponent<PreviewScript>();
        if (previewObj.canBuild && CheckForResources())
        {
            GameObject newObject = Instantiate(currentObject.prefab, currentPosition, Quaternion.Euler(currentRotation));
            village.AddBuilding(newObject);
            newObject.GetComponent<VillageBuilding>().ChangeFactionOwner(currentFaction);
            currentFaction.currentWood -= currentObject.woodCost;
            currentFaction.currentStone -= currentObject.stoneCost;
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
                currentFaction.currentWood += currentObject.woodRefund;
                currentFaction.currentStone += currentObject.stoneRefund;
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

}
