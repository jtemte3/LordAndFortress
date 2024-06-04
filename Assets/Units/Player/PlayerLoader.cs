using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public FactionEntityData factionEntityData;
    public LevelManager levelManager;
    public CustomUnitLoader customUnitLoader;
    public float loadDelay = 0.5f;
    private float DelayTime;
    // Start is called before the first frame update
    void Start()
    {
        DelayTime = Time.time + loadDelay;
    }

    private void Update()
    {
        if (Time.time >= DelayTime)
        {
            foreach (FactionObject faction in levelManager.factions)
            {
                if (factionEntityData.factionId.Equals(faction.factionId))
                {
                    customUnitLoader.LoadUnit(faction.customFactionObject.customUnits[0], faction.factionColor);
                }
            }

            this.enabled = false;
        }
    }
}
