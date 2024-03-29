using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillageBuilding : MonoBehaviour
{
    public string buildingId;
    public Vector3 location;
    public Vector3 coordinate;
    public Quaternion rotation;
    public float maxHealth;
    public float currentHealth;
    public VillageManager village;

    public void Start()
    {
        location = this.transform.position;
        rotation = this.transform.rotation;
        currentHealth = maxHealth;
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

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void SetVillage(VillageManager newVillage)
    {
        village = newVillage;
    }

    public VillageManager GetVillage()
    {
        return village;
    }
}


