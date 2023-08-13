using System.Collections.Generic;
using ItemSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Component
{
    public class ItemList: ListView
    {
        private readonly ItemDatabase _itemDatabase;
        private readonly List<Item> _valueList;
        public ItemList(ItemDatabase itemDatabase)
        {
            _itemDatabase = itemDatabase;
            _valueList = itemDatabase.GetItemList();
            makeItem = MakeItem;
            bindItem = AssignContent;
            itemsSource = _valueList;
            selectionType = SelectionType.Single;
            fixedItemHeight = 60;
        }

        private TemplateContainer MakeItem()
        {
            return new ItemDisplay(_itemDatabase).GetItemDisplay();
        }

        private void AssignContent(VisualElement display, int index)
        {
            if (display is TemplateContainer template)
            {
                Label idValue = template.Q<Label>("IdValue");
                Label nameValue = template.Q<Label>("NameValue");
                Item item = _itemDatabase.GetItemList()[index];
                idValue.text = item.id.ToString();
                nameValue.text = item.itemName;
            }
        }
    }
}