using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarracksMenuHandler : MonoBehaviour
{
    public Slider sldPurchaseMode;
    public TMP_Text lblPurchaseMode;
    public Button btnUnitOne;
    public Image unitOneImage; 
    public Button btnUnitTwo;
    public Image unitTwoImage;
    public Button btnUnitThree;
    public Image unitThreeImage;
    public Button btnUnitEng;

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