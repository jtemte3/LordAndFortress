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
    }

    public void saveGameState()
    {

    }
}
