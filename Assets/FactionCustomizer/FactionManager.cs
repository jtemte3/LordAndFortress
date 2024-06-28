using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class FactionManager : MonoBehaviour
{
    [Header("General Settings")]
    private CustomFactionObject loadedFaction;
    public bool loadFromFile = true;
    public GameObject unitUI;
    public GameObject factionUI;
    public UnitCustomizationController unitCustomizationController;
    public Camera screenShotCamera;

    [Header("Faction Settings")] 
    public TMP_InputField factionName;
    public List<Color> FactonColors = new();
    public Color currentColor;
    public Image heroImage;
    public Image unitOneImage;
    public Image unitTwoImage;
    public Image unitThreeImage;
    public GameObject colorIndicator;

    [Header("Unit Settings")]
    public GameObject customTroopPreview;
    public float speed = 0f;
    public Animator controller;
    public Button btnSaveUnit;
    public TMP_Text Lbl_UnitName;
    public Image currentUnitImage;


    // Start is called before the first frame update
    void Start()
    {
        LoadFactionDetails();
        unitCustomizationController.UpdateColor(currentColor);
        moveColorIndicator(findColorPosition(currentColor));
    }

    private void Update()
    {
        customTroopPreview.transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }

    public void SetFactionColor(int position)
    {
        moveColorIndicator(position);
        currentColor = FactonColors[position];
        unitCustomizationController.UpdateColor(currentColor);
    }

    private void moveColorIndicator(int position)
    {
        //use formula to determine new X value
        int newXPosition = -500 + (position * 125) + 960;
        colorIndicator.GetComponent<RectTransform>().position = new Vector3(newXPosition,
            colorIndicator.GetComponent<RectTransform>().position.y,
            colorIndicator.GetComponent<RectTransform>().position.z);
    }

    private int findColorPosition(Color currentColor)
    {
        int returnInt = -1;
        foreach(Color color in FactonColors)
        {
            if (currentColor == color)
            {
                returnInt = FactonColors.IndexOf(color);
            }
        }

        return returnInt;
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

        if (unitType.Equals(0))
        {
            CaptureImage("Icon-Hero.png");
        }
        else
        {
            CaptureImage("Icon-Unit-" + unitType + ".png");
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

            LoadUnitImages();
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
                currentUnitImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Hero.png");
            }
            else
            {
                Lbl_UnitName.text = "Unit " + unitType;
                currentUnitImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-" + unitType + ".png");
            }
        }
    }

    public void ExitUnitEditor()
    {
        if (loadedFaction != null)
        {
            customTroopPreview.SetActive(false);
            LoadUnitImages();
            unitUI.SetActive(false);
            factionUI.SetActive(true);
            btnSaveUnit.onClick.RemoveAllListeners();
        }
    }

    public void LoadUnitImages()
    {
        heroImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Hero.png");
        unitOneImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-1.png");
        unitTwoImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-2.png");
        unitThreeImage.sprite = new FileUtils().LoadSpriteFromFile("Icon-Unit-3.png");
    }

    public void CaptureImage(string fileName)
    {
        RenderTexture activeRenderTexture = new RenderTexture(600, 600, 24, RenderTextureFormat.ARGB32);
        screenShotCamera.targetTexture = activeRenderTexture;

        RenderTexture.active = activeRenderTexture;

        screenShotCamera.Render();

        Texture2D screenShot = new Texture2D(screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height, TextureFormat.ARGB32, false, true);
        screenShot.ReadPixels(new Rect(0, 0, screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height), 0, 0);
        screenShot.Apply();

        RenderTexture.active = null;

        new FileUtils().SaveImageToFile(screenShot, fileName);

        Destroy(screenShot);

        currentUnitImage.sprite = new FileUtils().LoadSpriteFromFile(fileName);
    }

}
