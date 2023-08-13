using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private ulong currentId = 0;
        [SerializeField] private List<ulong> ids;
        [SerializeField] private List<Item> items;
        
        private readonly Dictionary<ulong, Item> _database = new Dictionary<ulong, Item>();

        #region CRUD Operation

        public Item GetItem(ulong id)
        {
            return _database[id];
        }

        public void AddItem(Item newItem)
        {
            CheckItemExist(newItem);
            ulong assigningId = currentId++;
            newItem.id = assigningId;
            _database.Add(assigningId, newItem);
        }

        public void RemoveItem(ulong id)
        {
            CheckIdExist(id);
            _database.Remove(id);
        }

        public List<Item> GetItemList()
        {
            return _database.Values.ToList();
        }

        #endregion

        #region Utils

        private void CheckIdExist(ulong id)
        {
            if (!_database.Keys.ToList().Contains(id))
            {
                throw new Exception("Id doesn't exist. ID: " + id);
            }
        }

        private void CheckItemExist(Item item)
        {
            if (_database.Values.ToList().Contains(item))
            {
                throw new Exception("Duplicate item. Item name:" + item.itemName);
            }
        }

        #endregion

        #region Load & Save

        /**
         * Save database into scriptable object using serializable list
         */
        public void Save()
        {
            ids = _database.Keys.ToList();
            items = _database.Values.ToList();
            EditorUtility.SetDirty(this); // Mark the Scriptable Object as dirty to trigger serialization

            AssetDatabase.SaveAssets(); // Save the changes to the asset file
            AssetDatabase.Refresh();
        }
        
        /**
         * Must load before interact with database
         */
        public void Load()
        {
            _database.Clear();
            if (ids.Count != items.Count)
            {
                throw new Exception("Ids and items length not match. Require manual modification");
            }
            for (int i = 0; i < ids.Count; i++)
            {
                _database.Add(ids[i], items[i]);
            }
        }

        #endregion
    }
}