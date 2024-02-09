using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildableObject
{
    public string buildingName;
    public string buildingId;
    public GameObject prefab;
    public GameObject preview;
    public float buildingForwardOffset;
    public float buildingHorizontalOffset;
    public float buildHeightOffset;
}
