using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarracksMenuHandler : MonoBehaviour
{
    [Header("General")]
    public Slider sldPurchaseMode;
    public TMP_Text lblPurchaseMode;
    public string costFormat;
    [Header("Unit 1")]
    public Button btnUnitOne;
    public Image unitOneImage;
    public TMP_Text lblCost1;
    [Header("Unit 2")]
    public Button btnUnitTwo;
    public Image unitTwoImage;
    public TMP_Text lblCost2;
    [Header("Unit 3")]
    public Button btnUnitThree;
    public Image unitThreeImage;
    public TMP_Text lblCost3;
    [Header("Engineer")]
    public Button btnUnitEng;
    public TMP_Text lblCostEng;

    private void Start()
    {
        this.gameObject.SetActive(false);
        LoadUnitImages();
    }

    private void Update()
    {
        if (sldPurchaseMode.value.Equals(1))
        {
            lblPurchaseMode.text = "Offense";
        }
        else
        {
            lblPurchaseMode.text = "Defense";
        }
    }

    public void Setup(VillageManager village)
    {
        btnUnitOne.onClick.AddListener(() => village.SpawnUnit(1));
        btnUnitTwo.onClick.AddListener(() => village.SpawnUnit(2));
        btnUnitThree.onClick.AddListener(() => village.SpawnUnit(3));
        btnUnitEng.onClick.AddListener(() => village.SpawnUnit(4));

        lblCost1.text = string.Format(costFormat, village.GetCurrentFaction().customFactionObject.customUnits[1].unitGoldCost);
        lblCost2.text = string.Format(costFormat, village.GetCurrentFaction().customFactionObject.customUnits[2].unitGoldCost);
        lblCost3.text = string.Format(costFormat, village.GetCurrentFaction().customFactionObject.customUnits[3].unitGoldCost);
    }

    public void ClearListeners()
    {
        btnUnitOne.onClick.RemoveAllListeners();
        btnUnitTwo.onClick.RemoveAllListeners();
        btnUnitThree.onClick.RemoveAllListeners();
        btnUnitEng.onClick.RemoveAllListeners();
    }

    public void LoadUnitImages()
    {
        unitOneImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-1.png");
        unitTwoImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-2.png");
        unitThreeImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-3.png");
    }
}