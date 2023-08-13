using System;
using UnityEngine;

namespace ItemSystem
{
    /**
 * Building class to store building data
 */
    [Serializable]
    public class PlaceableObject : Item
    {
        public GameObject placeObject;
        public Vector2Int size;

        public PlaceableObject(ulong id, Sprite icon, string itemName, int buyPrice, int sellPrice, int stack, GameObject placeObject, Vector2Int size) : base(id, icon, itemName, buyPrice, sellPrice, stack)
        {
            this.placeObject = placeObject;
            this.size = size;
        }

        public PlaceableObject(ulong id, Sprite icon, string itemName, int buyPrice, int sellPrice, int stack, bool stackLocked, GameObject placeObject, Vector2Int size) : base(id, icon, itemName, buyPrice, sellPrice, stack, stackLocked)
        {
            this.placeObject = placeObject;
            this.size = size;
        }

        public Vector3Int[] CalculateTile(Vector3Int startPosition)
        {
            int tileNumber = size.x * size.y;
            Vector3Int[] result = new Vector3Int[tileNumber];
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    result[i * size.x + j] = new Vector3Int(startPosition.x + i, startPosition.y + j, 0);
                }
            }

            return result;
        }
    }
}