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

    // Start is called before the first frame update
    void Start()
    {
        factionJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/CurrentFaction.json";
        Debug.Log("FactionManager json path:" + factionJsonPath);

        LoadFactionDetails();
        unitCustomization.UpdateColor(currentColor);
    }

    public void SetFactionColor(int position)
    {
        currentColor = FactonColors[position];
        unitCustomization.UpdateColor(currentColor);
    }

    public void SaveFactionDetails()
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

    public void LoadFactionDetails()
    {
        StreamReader reader = new(factionJsonPath);
        string configJson = reader.ReadToEnd();

        CustomFactionObject loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);
        factionName.text = loadedFaction.name;
        currentColor = loadedFaction.color;
        unitCustomization.LoadUnit(loadedFaction.customUnits[0]);
        unitCustomization.UpdateColor(currentColor);

        reader.Close();
    }

}
