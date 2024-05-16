using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactionObject
{
    public string factionName;
    public string factionId;
    public string teamId;
    public Color factionColor;
    public CustomFactionObject customFactionObject;
    public int ownedFlags;
    public int currentWood;
    public int currentStone;
    public int currentGold;
    public int currentFood;
    public int currentPopulation;
}
