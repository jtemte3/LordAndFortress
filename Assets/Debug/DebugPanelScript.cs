using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPanelScript : MonoBehaviour
{
    public GameObject debugPanel;
    public TMP_Text fpsLabel;
    private string fpsLabelFormat;
    public float refreshRate = .5f;
    private float timer = 0;
    public bool isPanelEnabled;

    private void Start()
    {
        fpsLabelFormat = fpsLabel.text;
        debugPanel.SetActive(isPanelEnabled);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isPanelEnabled)
            {
                isPanelEnabled = false;
                debugPanel.SetActive(false);
            }
            else
            {
                isPanelEnabled = true;
                debugPanel.SetActive(true);
            }
            
        }

        if (isPanelEnabled)
        {
            if (Time.unscaledTime > timer)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                fpsLabel.text = string.Format(fpsLabelFormat, fps);
                timer = Time.unscaledTime + refreshRate;
            }
        }
    }

    public void WriteVillageDataToFile()
    {
        BuildSystemController playerBuildController = FindObjectOfType<BuildSystemController>();
        VillageDataObject villageDataObject = new();

        //f = factionId
        //v = villageId
        //{3} = timestamp
        //string villageDataFileNameFormat = "f{0}_v{1}_{3}";

        if (playerBuildController.currentVillage != null)
        {
            VillageManager village = playerBuildController.currentVillage;

            villageDataObject.villageSize = village.GetGridSize();
            villageDataObject.buildings = new List<BuildingDataObject>();

            foreach (KeyValuePair<Vector3,GameObject> building in village.villageMap)
            {
                //Add all buildiings but the flag.. the flag is apart of the prefab
                if(!building.Key.Equals(new Vector3(0, 0, 0)))
                {
                    building.Value.GetComponent<VillageBuilding>();

                    BuildingDataObject buildingData = new(
                        building.Value.GetComponent<VillageBuilding>().GetBuildingId(),
                        building.Key,
                        building.Value.GetComponent<VillageBuilding>().GetRotation(),
                        building.Value.GetComponent<VillageBuilding>().GetMaxHealth());

                    villageDataObject.buildings.Add(buildingData);
                }
            }

            new FileUtils().SaveVillageToFile(villageDataObject, "testVillage.json");
        }
    }
}
