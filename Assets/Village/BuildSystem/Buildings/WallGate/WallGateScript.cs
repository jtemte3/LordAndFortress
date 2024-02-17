using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGateScript : MonoBehaviour
{

    public VillageBuilding gateBuilding;
    public Animator animator;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<FactionEntityData>())
        {
            FactionEntityData entity = other.gameObject.GetComponent<FactionEntityData>();
            if (entity.factionId == gateBuilding.factionId)
            {
                animator.SetBool("doorOpen", true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<FactionEntityData>())
        {
            FactionEntityData entity = other.gameObject.GetComponent<FactionEntityData>();
            if (entity.factionId == gateBuilding.factionId)
            {
                animator.SetBool("doorOpen", false);
            }
        }
    }
}
