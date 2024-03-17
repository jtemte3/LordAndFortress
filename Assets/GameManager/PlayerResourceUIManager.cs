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

    private string factionFormat;
    private string woodFormat;
    private string foodFormat;
    private string populationFormat;
    private string goldFormat;
    private string flagFormat;

    private void Start()
    {
        string factionId = entityData.factionId;

        factionFormat = LblFaction.text;
        woodFormat = LblWood.text;
        foodFormat = LblFood.text;
        populationFormat = LblPopulation.text;
        goldFormat = LblGold.text;
        flagFormat = LblFlagCount.text;

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
        LblFaction.text = string.Format(factionFormat, currentFaction.factionName);
        LblWood.text = string.Format(woodFormat, currentFaction.currentWood);
        LblFood.text = string.Format(foodFormat, currentFaction.currentFood);
        LblPopulation.text = string.Format(populationFormat, currentFaction.currentPopulation);
        LblGold.text = string.Format(goldFormat, currentFaction.currentGold);
        LblFlagCount.text = string.Format(flagFormat, currentFaction.ownedFlags , gameManager.villages.Count);
    }
}
