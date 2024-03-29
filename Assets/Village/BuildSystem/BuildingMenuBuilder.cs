using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingMenuBuilder : MonoBehaviour
{
    public LevelManager gameManager;
    public BuildSystemController buildSystem;
    public string buildingId;
    public Image buildingImage;
    public TMP_Text buildingName;
    public TMP_Text buildingCost;
    public Button buildingButton;
    private string buildingCostFormat;

    // Start is called before the first frame update
    void Start()
    {
        buildingCostFormat = buildingCost.text;
        buildingButton.onClick.AddListener(() => buildSystem.ChangeBuildObject(buildingId));

        foreach (BuildableObject building in gameManager.objects)
        {
            if (building.buildingId.Equals(buildingId))
            {
                buildingName.text = building.buildingName;
                buildingCost.text = string.Format(buildingCostFormat, building.woodCost);
                buildingImage.sprite = building.buildingImage;
            }
        }
    }
}
