using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Building List")]
public class BuildingList : ScriptableObject
{
    public List<PlaceableObject> list;
    public Dictionary<int, PlaceableObject> BuildingDatabase;

    public void InitializeBuildingDatabase(int startId)
    {
        foreach (var building in list)
        {
            BuildingDatabase.Add(startId++, building);
        }
    }
}
