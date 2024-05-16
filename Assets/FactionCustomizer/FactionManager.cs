using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactionManager : MonoBehaviour
{
    public UnitCustomizationController unitCustomizationController;
    private CustomFactionObject loadedFaction;
    public TMP_InputField factionName;
    public List<Color> FactonColors = new();
    public Color currentColor;
    public bool loadFromFile = true;
    public GameObject customTroopPreview;
    public float speed = 0f;
    public Animator controller;
    public Image heroImage;
    public Image unitOneImage;
    public Image unitTwoImage;
    public Image unitThreeImage;
    public GameObject unitUI;
    public GameObject factionUI;
    public Button btnSaveUnit;
    public TMP_Text Lbl_UnitName;

    // Start is called before the first frame update
    void Start()
    {
        LoadFactionDetails();
        unitCustomizationController.UpdateColor(currentColor);
    }

    private void Update()
    {
        customTroopPreview.transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }

    public void SetFactionColor(int position)
    {
        currentColor = FactonColors[position];
        unitCustomizationController.UpdateColor(currentColor);
    }

    public void RotateCharacter(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetCharacterRotation()
    {
        customTroopPreview.transform.rotation = Quaternion.Euler(0, 180, 0);
        speed = 0;
    }

    public void ChangeCharacterPose()
    {
        if (controller.GetBool("Ready").Equals(true))
        {
            controller.SetBool("Ready", false);
        }
        else
        {
            controller.SetBool("Ready", true);
        }
    }

    public void SaveFactionDetails()
    {
        if (loadFromFile)
        {
            if (loadedFaction != null)
            {
                loadedFaction.name = factionName.text;
                loadedFaction.color = currentColor;
                new FileUtils().SaveFactionToFile(loadedFaction);
            }
            else
            {
                CustomFactionObject newFaction = new();
                newFaction.name = factionName.text;
                newFaction.color = currentColor;
                newFaction.customUnits = new List<CustomUnitObject>(4);
                newFaction.customUnits[0] = unitCustomizationController.ExportUnit();
                newFaction.customUnits[1] = unitCustomizationController.ExportUnit();
                newFaction.customUnits[2] = unitCustomizationController.ExportUnit();
                newFaction.customUnits[3] = unitCustomizationController.ExportUnit();
                new FileUtils().SaveFactionToFile(newFaction);
            }
            

        }
        else
        {
            Debug.Log("Mocking Save Function");
        }
        
    }
    public void SaveUnitDetails(int unitType)
    {
        if (loadedFaction != null)
        {
            loadedFaction.customUnits[unitType] = unitCustomizationController.ExportUnit();
            new FileUtils().SaveFactionToFile(loadedFaction);
        }
        else
        {
            CustomFactionObject newFaction = new();
            newFaction.name = factionName.text;
            newFaction.color = currentColor;
            newFaction.customUnits = new List<CustomUnitObject>(4);
            newFaction.customUnits[unitType] = unitCustomizationController.ExportUnit();

            new FileUtils().SaveFactionToFile(newFaction);
        }

    }

    public void LoadFactionDetails()
    {
        if (loadFromFile)
        {
            loadedFaction = new FileUtils().LoadFactionFromFile();
            factionName.text = loadedFaction.name;
            currentColor = loadedFaction.color;
            unitCustomizationController.LoadUnit(loadedFaction.customUnits[0]);
            unitCustomizationController.UpdateColor(loadedFaction.color);

            heroImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Hero.png");
            unitOneImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-1.png");
            unitTwoImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-2.png");
            unitThreeImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-3.png");
        }
        else
        {
            Debug.Log("Mocking Load Function");
        }

    }

    public void LoadUnitEditor(int unitType)
    {
        if (loadedFaction != null)
        {
            unitCustomizationController.LoadUnit(loadedFaction.customUnits[unitType]);
            unitCustomizationController.UpdateColor(currentColor);
            factionUI.SetActive(false);
            unitUI.SetActive(true);
            customTroopPreview.SetActive(true);
            btnSaveUnit.onClick.AddListener(() => SaveUnitDetails(unitType));
            if (unitType.Equals(0))
            {
                Lbl_UnitName.text = "Leader";
            }
            else
            {
                Lbl_UnitName.text = "Unit " + unitType;
            }
        }
    }

    public void ExitUnitEditor()
    {
        if (loadedFaction != null)
        {
            customTroopPreview.SetActive(false);
            unitUI.SetActive(false);
            factionUI.SetActive(true);
            btnSaveUnit.onClick.RemoveAllListeners();
        }
    }

}
