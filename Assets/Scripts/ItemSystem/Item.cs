using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ItemSystem
{
    [Serializable]
    public class Item
    {
        public static int MaxStackAmount = 512;
        
        [FormerlySerializedAs("ID")] public ulong id;
        [FormerlySerializedAs("Icon")] public Sprite icon;
        [FormerlySerializedAs("ItemName")] public string itemName;
        [FormerlySerializedAs("BuyPrice")] public int buyPrice;
        [FormerlySerializedAs("SellPrice")] public int sellPrice;
        
        private int _stack;
        private readonly bool _stackLocked = false;

        public Item(ulong id, Sprite icon, string itemName, int buyPrice, int sellPrice, int stack)
        {
            this.id = id;
            this.icon = icon;
            this.itemName = itemName;
            this.buyPrice = buyPrice;
            this.sellPrice = sellPrice;
            _stack = stack;
        }

        public Item(ulong id, Sprite icon, string itemName, int buyPrice, int sellPrice, int stack, bool stackLocked)
        {
            this.id = id;
            this.icon = icon;
            this.itemName = itemName;
            this.buyPrice = buyPrice;
            this.sellPrice = sellPrice;
            _stack = stack;
            _stackLocked = stackLocked;
        }

        public int Stack
        {
            get => _stack;
            set
            {
                if (value <= 0)
                {
                    _stack = 0;
                    return;
                }

                _stack = value;
            }
        }

        public void AddToStack(int amount)
        {
            if (_stack + amount > MaxStackAmount || _stackLocked)
            {
                return;
            }
            _stack += amount;
        }

        public void RemoveFromStack(int amount)
        {
            if (_stack - amount < 0)
            {
                return;
            }

            _stack -= amount;
        }

        protected bool Equals(Item other)
        {
            return Equals(icon, other.icon) && itemName == other.itemName && buyPrice == other.buyPrice && sellPrice == other.sellPrice && _stackLocked == other._stackLocked;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, icon, itemName, buyPrice, sellPrice, _stack, _stackLocked);
        }
    }
}