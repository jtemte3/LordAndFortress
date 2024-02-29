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
    public string factionJsonPath;
    public bool loadFromFile = true;
    public Transform customTroopPreviewTransform;
    public float speed = 0f;
    public Animator controller;

    // Start is called before the first frame update
    void Start()
    {
        factionJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/CurrentFaction.json";
        Debug.Log("FactionManager json path:" + factionJsonPath);

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
            StreamWriter writer = new StreamWriter(factionJsonPath);

            CustomFactionObject newFaction = new();
            newFaction.name = factionName.text;
            newFaction.color = currentColor;
            newFaction.customUnits = new List<CustomUnitObject>();
            newFaction.customUnits.Add(unitCustomization.ExportUnit());

            string factionJson = JsonUtility.ToJson(newFaction, true);

            writer.Write(factionJson);
            writer.Flush();
            writer.Close();
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
            StreamReader reader = new(factionJsonPath);
            string configJson = reader.ReadToEnd();

            CustomFactionObject loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);
            factionName.text = loadedFaction.name;
            currentColor = loadedFaction.color;
            unitCustomization.LoadUnit(loadedFaction.customUnits[0]);
            unitCustomization.UpdateColor(loadedFaction.color);

            reader.Close();
        }
        else
        {
            Debug.Log("Mocking Load Function");
        }

    }

}
