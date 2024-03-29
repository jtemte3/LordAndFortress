using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingDataObject
{
    public string buildingId;
    public Vector3 coordinate;
    public Quaternion rotation;
    public float maxHealth;

    public BuildingDataObject(string buildingId, Vector3 coordinate, Quaternion rotation, float maxHealth)
    {
        this.buildingId = buildingId;
        this.coordinate = coordinate;
        this.rotation = rotation;
        this.maxHealth = maxHealth;
    }

    public void SetBuildingId(string id)
    {
        buildingId = id;
    }

    public string GetBuildingId()
    {
        return buildingId;
    }

    public void SetCoordinate(Vector3 newCoordinate)
    {
        coordinate = newCoordinate;
    }

    public Vector3 GetCoordinate()
    {
        return coordinate;
    }

    public void SetRotation(Quaternion newQuaternion)
    {
        rotation = newQuaternion;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public void SetMaxHealth(float newValue)
    {
        maxHealth = newValue;
    }

    public float GetMaxHeatlth()
    {
        return maxHealth;
    }
}


