using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewScript : MonoBehaviour
{
    public List<Collider> colList = new List<Collider>();
    public string layerMaskName;
    //public LayerMask layer; // layer for placed items
    public Renderer previewRenderer;
    public Color valid = new Color(0,1,0,.25f);
    public Color invalid = new Color(1, 0, 0, .25f);
    public bool canBuild;

    // Update is called once per frame
    private void Update()
    {
        setBuildStatus();
        setColor();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer(layerMaskName))
        {
            colList.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerMaskName))
        {
            colList.Remove(other);
        }
    }

    private void setBuildStatus()
    {
        if (colList.Count.Equals(0))
        {
            canBuild = true;
        }
        else
        {
            canBuild = false;
        }
    }

    private void setColor()
    {
        if (!canBuild)
        {
            previewRenderer.material.color = invalid;
        }
        else
        {
            previewRenderer.material.color = valid;
        }
    }
}
