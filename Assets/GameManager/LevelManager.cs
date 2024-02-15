using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<BuildableObject> objects = new();
    public List<VillageManager> villages = new();
    public List<FactionObject> factions = new();

    // Start is called before the first frame update
    void Start()
    {
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

    public void saveGameState()
    {

    }
}
