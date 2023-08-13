using System.Collections.Generic;
using ItemSystem;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Component
{
    public class ItemDisplay: GroupBox
    {
        private readonly TemplateContainer _template;
        private readonly Label _idDisplay;
        private readonly Label _nameDisplay;

        private readonly ItemDatabase _itemDatabase;
        private readonly List<Item> _itemsList;

        public ItemDisplay(ItemDatabase itemDatabase)
        {
            VisualTreeAsset root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/UIToolkit Assets/ItemDatabaseWindow/item_display.uxml");
            _template = root.Instantiate();
            _idDisplay = _template.Q<Label>("IdDisplay");
            _nameDisplay = _template.Q<Label>("IdDisplay");

            _itemDatabase = itemDatabase;
            _itemsList = itemDatabase.GetItemList();

            _template.style.height = 60;
            _template.style.flexGrow = 1;
        }

        public void AssignContent(int index)
        {
            _idDisplay.text = _itemsList[index].id.ToString();
            _nameDisplay.text = _itemsList[index].itemName;
            name = _itemsList[index].id.ToString();
        }

        public TemplateContainer GetItemDisplay()
        {
            return _template;
        }
    }
}