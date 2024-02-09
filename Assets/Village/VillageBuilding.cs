using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillageBuilding : MonoBehaviour
{
    public string buildingId;
    public Vector3 location;
    public Quaternion rotation;
    public float maxHealth;
    public float currentHealth;

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

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
}


