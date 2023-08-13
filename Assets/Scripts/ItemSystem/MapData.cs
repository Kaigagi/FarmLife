using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    public Dictionary<Vector3Int, PlaceableObject> BuildingList1 { get; } = new Dictionary<Vector3Int, PlaceableObject>();
    
    
}
