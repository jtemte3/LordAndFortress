using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using TMPro;

public class FactionManager : MonoBehaviour
{
    public UnitCustomizationController unitCustomization;
    public TMP_InputField factionName;
    public List<Color> FactonColors = new();
    public Color currentColor;
    public bool loadFromFile = true;
    public Transform customTroopPreviewTransform;
    public float speed = 0f;
    public Animator controller;

    // Start is called before the first frame update
    void Start()
    {
        LoadFactionDetails();
        unitCustomization.UpdateColor(currentColor);
    }

    private void Update()
    {
        customTroopPreviewTransform.Rotate(Vector3.up, Time.deltaTime * speed);
    }

    public void SetFactionColor(int position)
    {
        currentColor = FactonColors[position];
        unitCustomization.UpdateColor(currentColor);
    }

    public void RotateCharacter(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetCharacterRotation()
    {
        customTroopPreviewTransform.rotation = Quaternion.Euler(0, 180, 0);
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
            CustomFactionObject newFaction = new();
            newFaction.name = factionName.text;
            newFaction.color = currentColor;
            newFaction.customUnits = new List<CustomUnitObject>();
            newFaction.customUnits.Add(unitCustomization.ExportUnit());

            new FileUtils().SaveFactionToFile(newFaction);

        }
        else
        {
            Debug.Log("Mocking Save Function");
        }
        
    }

    public void LoadFactionDetails()
    {
        if (loadFromFile)
        {
            CustomFactionObject loadedFaction = new FileUtils().LoadFactionFromFile();
            factionName.text = loadedFaction.name;
            currentColor = loadedFaction.color;
            unitCustomization.LoadUnit(loadedFaction.customUnits[0]);
            unitCustomization.UpdateColor(loadedFaction.color);
        }
        else
        {
            Debug.Log("Mocking Load Function");
        }

    }

}
