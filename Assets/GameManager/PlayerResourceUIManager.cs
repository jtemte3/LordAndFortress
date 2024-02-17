using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerResourceUIManager : MonoBehaviour
{
    public FactionEntityData entityData;
    public LevelManager gameManager;

    FactionObject currentFaction;

    public Image img_Color;
    public TMP_Text LblFaction;
    public TMP_Text LblWood;
    public TMP_Text LblFood;
    public TMP_Text LblPopulation;
    public TMP_Text LblGold;
    public TMP_Text LblFlagCount;

    private void Start()
    {
        string factionId = entityData.factionId;

        foreach (FactionObject faction in gameManager.factions)
        {
            if (factionId.Equals(faction.factionId))
            {
                currentFaction = faction;
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        img_Color.color = currentFaction.factionColor;
        LblFaction.text = currentFaction.factionName;
        LblWood.text = "Wood: " + currentFaction.currentWood;
        LblFood.text = "Food: " + currentFaction.currentFood;
        LblPopulation.text = "Population: " + currentFaction.currentPopulation;
        LblGold.text = "Gold: " + currentFaction.currentGold;
        LblFlagCount.text = "Captured Flags: " + currentFaction.ownedFlags + "/" + gameManager.villages.Count;
    }
}
