using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    int x, z;
    GameObject pointMesh;
    BuildGridHandler gridManager;
    //RoomData room = null;
    public enum PointTypes { Empty, InUse};
    PointTypes pointType = PointTypes.Empty;

    public int GetX()
    {
        return x;
    }

    public void SetX(int newX)
    {
        x = newX;
    }

    public int GetZ()
    {
        return z;
    }

    public void SetZ(int newZ)
    {
        z = newZ;
    }

    public string GetCoodinate()
    {
        return x+","+z;
    }

    public void SetPointObject(GameObject newObj)
    {
        GameObject toDestroy = this.pointMesh;
        pointMesh = newObj;
        GameObject.Destroy(toDestroy,1f);
    }

    public void SetPointManager(BuildGridHandler newManager)
    {
        gridManager = newManager;
    }

    public BuildGridHandler GetPointManager()
    {
        return gridManager;
    }

    public void DisableRenderer()
    {
        this.pointMesh.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void EnableRenderer()
    {
        this.pointMesh.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void SetColor(Color newColor)
    {
        pointMesh.GetComponent<Renderer>().material.color = newColor;
    }

    public void SetPointType(PointTypes newType)
    {
        pointType = newType;
    }

    public PointTypes getPointType()
    {
        return pointType;
    }

    /*public void SetRoom(RoomData newRoom)
    {
        room = newRoom;
    }
    public RoomData GetRoom()
    {
        return room;
    }*/
}
