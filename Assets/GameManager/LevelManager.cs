using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<BuildableObject> objects = new List<BuildableObject>();
    public List<VillageManager> villages = new List<VillageManager>();
    // Start is called before the first frame update
    void Start()
    {
        villages.AddRange(FindObjectsOfType<VillageManager>());
    }

    public void saveGameState()
    {

    }
}
