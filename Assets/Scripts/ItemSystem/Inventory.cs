using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Inventory")]
    public class Inventory : ScriptableObject
    {
        private readonly Dictionary<Vector2Int, Item> _itemStorage = new Dictionary<Vector2Int, Item>();
        private readonly Wallet _wallet = new Wallet();
        private readonly QuickItemBar _quickItemBar = new QuickItemBar();

        public Dictionary<Vector2Int, Item> ItemStorage => _itemStorage;
        public Wallet Wallet => _wallet;
        public QuickItemBar QuickItemBar => _quickItemBar;

        public Item GetItemByGridPos(Vector2Int position)
        {
            return _itemStorage[position];
        }
    }
}