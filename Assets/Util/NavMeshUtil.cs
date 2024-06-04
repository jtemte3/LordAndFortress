using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshUtil : MonoBehaviour
{
    public NavMeshSurface surface;
    public float refreshRate;
    private float nextTime = 0.0f;
    public bool hasChange = false;
    private bool isTrriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + refreshRate;
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasChange == true)
        {
            nextTime = Time.time + refreshRate;
            isTrriggered = true;
            hasChange = false;
        }

        if (Time.time > nextTime && isTrriggered == true)
        {
            isTrriggered = false;
            surface.UpdateNavMesh(surface.navMeshData);
        }
    }
}
