using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<BuildableObject> objects = new();
    public List<VillageManager> villages = new();
    public List<FactionObject> factions = new();
    private string factionJsonPath;
    public bool showCursor;

    // Start is called before the first frame update
    void Start()
    {
        factionJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/CurrentFaction.json";
        Debug.Log("LevelManager json path:" + factionJsonPath);
        villages.AddRange(FindObjectsOfType<VillageManager>());

        foreach (VillageManager village in villages)
        {
            foreach (FactionObject faction in factions)
            {
                if (village.currentFactionId == faction.factionId)
                {
                    faction.ownedFlags++;
                }
            }
        }

        LoadFaction("0001");

        //Lock the Cursor
        Cursor.lockState = CursorLockMode.Locked;
        //Set Cursor to not be visible
        Cursor.visible = false;
        showCursor = false;

        Debug.Log("End of Start() function, logging final list factions");
        foreach (FactionObject faction in factions)
        {
            Debug.Log("factionId: " + faction.factionId + " factionName: "+faction.factionName);
        }

    }

    public void Update()
    {
        if (showCursor)
        {
            //Unlock the Cursor
            Cursor.lockState = CursorLockMode.None;
            //Set Cursor to be visible
            Cursor.visible = true;
        }
        else
        {
            //Lock the Cursor
            Cursor.lockState = CursorLockMode.Locked;
            //Set Cursor to not be visible
            Cursor.visible = false;
        }
    }

    public void ChangeVillageOwnership(string oldFactionId, string newFactionId)
    {
        foreach (FactionObject faction in factions)
        {
            if (oldFactionId == faction.factionId)
            {
                faction.ownedFlags--;
            }
            if (newFactionId == faction.factionId)
            {
                faction.ownedFlags++;
            }
        }
    }

    public void LoadFaction(string factionId)
    {
        foreach (FactionObject faction in factions)
        {
            if (factionId == faction.factionId)
            {
                StreamReader reader = new(factionJsonPath);
                string configJson = reader.ReadToEnd();

                CustomFactionObject loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);

                Debug.Log("Level Manager: loadedFactionName :" + loadedFaction.name);

                faction.factionName = loadedFaction.name;
                faction.factionColor = loadedFaction.color;

                Debug.Log("Level Manager: factionName :" + faction.factionName);

                reader.Close();
                break;
            }
        }
    }
}
