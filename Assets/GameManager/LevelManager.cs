using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<BuildableObject> objects = new();
    public List<VillageManager> villages = new();
    public List<FactionObject> factions = new();
    public List<FactionEntityData> Heroes = new();
    public bool showCursor;
    public NavMeshUtil navMesh;

    // Start is called before the first frame update
    void Start()
    {
        villages.AddRange(FindObjectsOfType<VillageManager>());

        LoadFaction("0001");

        foreach (VillageManager village in villages)
        {
            foreach (FactionObject faction in factions)
            {
                if (village.currentFactionId == faction.factionId)
                {
                    faction.ownedFlags++;

                    SetupFlagMaterial(village.bannerFlag);

                    village.ChangeVillageFlagColor(faction.factionColor);
                }
            }
        }

        

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
                CustomFactionObject loadedFaction = new FileUtils().LoadFactionFromFile();

                Debug.Log("Level Manager: loadedFactionName :" + loadedFaction.name);

                faction.factionName = loadedFaction.name;
                faction.factionColor = loadedFaction.color;
                faction.customFactionObject = loadedFaction;

                foreach (FactionEntityData hero in Heroes)
                {
                    if (hero.factionId == factionId)
                    {
                        faction.hero = hero.gameObject;
                    }
                }

                Debug.Log("Level Manager: factionName :" + faction.factionName);
                break;
            }
        }
    }

    public void SetCursor(bool state)
    {
        showCursor = state;
    }

    private void SetupFlagMaterial(Renderer banner)
    {
        banner.material = new Material(banner.material);
    }
}
