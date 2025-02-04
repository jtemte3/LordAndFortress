using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPointManager : MonoBehaviour
{
    public LayerMask structureMask;
    public float physicsRadius;
    public void AttemptVisibility()
    {
        Collider[] overlappingStructures = Physics.OverlapSphere(transform.position, physicsRadius, structureMask);
        if (overlappingStructures.Length == 0)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void DisableVisibility()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
