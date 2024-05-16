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
    public string capturingFactiton;
    public int highestNumberOfUnits = 0;

    // Update is called once per frame
    void Update()
    {
        if (currentFactionContenders.Count > 0)
        {
            foreach (string faction in currentFactionContenders.Keys)
            {
                int unitCount = currentFactionContenders[faction];
                if (unitCount > highestNumberOfUnits)
                {
                    highestNumberOfUnits = unitCount;
                    capturingFactiton = faction;
                    flagAnimator.speed = 1;
                }
                if (faction != capturingFactiton && unitCount == highestNumberOfUnits)
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
            highestNumberOfUnits = 0;
            capturingFactiton = villageManager.currentFactionId;
            isBeingCaptured = false;
            flagAnimator.SetBool("bannerDown", false);
            villageManager.ChangeVillageFlagColor(capturingFactiton);
        }

        if (isBeingCaptured && capturingFactiton != villageManager.currentFactionId)
        {            
            if (Time.time >= finishCaptureTime)
            {
                //Flag capture time achieved and flag was captured by an enemy
                isBeingCaptured = false;
                if (capturingFactiton != villageManager.currentFactionId)
                {
                    villageManager.gameManager.ChangeVillageOwnership(villageManager.currentFactionId, capturingFactiton);
                    villageManager.ChangeVillageFactionOwner(capturingFactiton);
                }
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
        else
        {
            flagAnimator.SetBool("bannerDown", false);
        }

        if (!isBeingCaptured)
        {
            flagAnimator.SetBool("bannerDown", false);
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

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<BuildSystemController>() && isBeingCaptured != true)
        {
            other.gameObject.GetComponent<BuildSystemController>().currentVillage = villageManager;
            other.gameObject.GetComponent<BuildSystemController>().canBuild = true;
        }
    }*/

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
                    capturingFactiton = villageManager.currentFactionId;
                    isBeingCaptured = false;
                    flagAnimator.SetBool("bannerDown", false);
                    villageManager.ChangeVillageFlagColor(capturingFactiton);
                }
            }
        }
    }
}
