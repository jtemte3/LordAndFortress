using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageCaptureZoneScript : MonoBehaviour
{
    public VillageManager villageManager;
    public Animator flagAnimator;
    Dictionary<string, int> currentFactionContenders = new();
    public bool isBeingCaptured = false;
    public float captureTime;
    float finishCaptureTime;
    float flagChangeTime;
    string capturingFactiton;

    // Update is called once per frame
    void Update()
    {
        if (currentFactionContenders.Count > 0)
        {            
            int highestNumberOfUnits = 0;

            foreach (string faction in currentFactionContenders.Keys)
            {
                int unitCount = currentFactionContenders[faction];
                if (unitCount > highestNumberOfUnits)
                {
                    capturingFactiton = faction;
                    flagAnimator.speed = 1;
                }
                if (unitCount == highestNumberOfUnits)
                {
                    isBeingCaptured = false;
                    flagAnimator.speed = 0;
                }
            }

            if (!isBeingCaptured && capturingFactiton != villageManager.currentFactionId)
            {
                finishCaptureTime = Time.time + captureTime;
                flagChangeTime = Time.time + (captureTime / 2);
                isBeingCaptured = true;
            }
        }
        else
        {
            isBeingCaptured = false;
            capturingFactiton = villageManager.currentFactionId;
        }

        if (isBeingCaptured)
        {            
            if (Time.time >= finishCaptureTime)
            {
                isBeingCaptured = false;
                villageManager.gameManager.ChangeVillageOwnership(villageManager.currentFactionId, capturingFactiton);
                villageManager.ChangeVillageFactionOwner(capturingFactiton);
                
            }
            if (Time.time < flagChangeTime && !flagAnimator.GetBool("bannerDown"))
            {
                flagAnimator.SetBool("bannerDown", true);
            }
            if (Time.time >= flagChangeTime && flagAnimator.GetBool("bannerDown"))
            {
                villageManager.ChangeVillageFlagColor(capturingFactiton);
                flagAnimator.SetBool("bannerDown", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FactionEntityData>())
        {
            string factionId = other.gameObject.GetComponent<FactionEntityData>().factionId;
            int numberOfFactionContenders = 1;

            if (currentFactionContenders.ContainsKey(factionId))
            {
                numberOfFactionContenders = currentFactionContenders[factionId] + 1;
                currentFactionContenders[factionId] = numberOfFactionContenders;
            }
            else
            {
                currentFactionContenders.Add(factionId, numberOfFactionContenders);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<BuildSystemController>() && isBeingCaptured != true)
        {
            other.gameObject.GetComponent<BuildSystemController>().currentVillage = villageManager;
            other.gameObject.GetComponent<BuildSystemController>().canBuild = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<FactionEntityData>())
        {
            string factionId = other.gameObject.GetComponent<FactionEntityData>().factionId;

            if (currentFactionContenders.ContainsKey(factionId))
            {
                int numberOfFactionContenders = currentFactionContenders[factionId] - 1;
                if (numberOfFactionContenders > 0)
                {
                    currentFactionContenders[factionId] = numberOfFactionContenders;
                }
                else
                {
                    currentFactionContenders.Remove(factionId);
                }
                
            }
        }
    }
}
