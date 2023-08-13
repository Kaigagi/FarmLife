using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Editor.Component;
using ItemSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor
{
    public class ItemDatabaseWindow : EditorWindow
    {
        private ItemDatabase _itemDatabase;
        private readonly List<string> _itemTypes = new List<string>()
        {
            "Item",
            "Placeable Item",
        };

        #region Component Variables

        private SliderInt _buyPriceSlider;
        private SliderInt _sellPriceSlider;
        private DropdownField _itemTypeInput;
        private ObjectField _iconInput;
        private TextField _nameInput;
        private ObjectField _placeObjectInput;
        private Vector2Field _sizeInput;
        private Button _addButton;
        private Button _removeButton;
        private Toggle _stackableInput;
        private GroupBox _inputArea;
        private VisualElement _listArea;

        #endregion

        #region Item Current Data

        private string _currentName;
        private string _currentItemType;
        private Sprite _currentIcon;
        private int _currentBuy;
        private int _currentSell;
        private GameObject _currentPlaceObject;
        private Vector2Int _currentSize;
        private bool _currenStackable;

        #endregion
        
        [MenuItem("Window/Item Database")]
        private static void ShowWindow()
        {
            var window = GetWindow<ItemDatabaseWindow>();
            window.titleContent = new GUIContent("Item Database");
            window.Show();
        }

        private void OnEnable()
        {
            PrepareDatabase();
            PrepareRootElement();
            PrepareComponent();
            RegisterOnChangeEvents();
            
            _addButton.clicked += OnAddItem;
        }

        private void OnDisable()
        {
            _itemDatabase.Save();
        }

        #region Prepare

        private void PrepareDatabase()
        {
            _itemDatabase =
                AssetDatabase.LoadAssetAtPath<ItemDatabase>("Assets/Scripts/ScriptableObjects/ItemDatabase.asset");
            _itemDatabase.Load();
        }

        private void PrepareRootElement()
        {
            VisualTreeAsset root =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/UIToolkit Assets/ItemDatabaseWindow/item_database.uxml");
            TemplateContainer container = root.CloneTree();
            rootVisualElement.Add(container);
        }

        private void PrepareComponent()
        {
            _inputArea = rootVisualElement.Q<GroupBox>("InputArea");
            _itemTypeInput = rootVisualElement.Q<DropdownField>("ItemType");
            _iconInput = AddObjectFieldToElement(_inputArea, 1, "Icon", typeof(Sprite));
            _nameInput = rootVisualElement.Q<TextField>("ItemName");
            _buyPriceSlider = rootVisualElement.Q<SliderInt>("BuyPriceSlider");
            _sellPriceSlider = rootVisualElement.Q<SliderInt>("SellPriceSlider");
            _stackableInput = rootVisualElement.Q<Toggle>("StackableInput");
            _addButton = rootVisualElement.Q<Button>("AddButton");
            _removeButton = rootVisualElement.Q<Button>("RemoveButton");
            _itemTypeInput.choices = _itemTypes;
            _itemTypeInput.value = _itemTypes[0];
            _currentItemType = _itemTypes[0];
            _listArea = rootVisualElement.Q<VisualElement>("ListArea");
            _listArea.Add(new ItemList(_itemDatabase));
        }

        private void RegisterOnChangeEvents()
        {
            RegisterOnSliderChange(OnBuyPriceChange, _buyPriceSlider);
            RegisterOnSliderChange(OnSellPriceChange, _sellPriceSlider);
            _itemTypeInput.RegisterValueChangedCallback(OnItemTypeChange);
            _nameInput.RegisterValueChangedCallback(OnNameChange);
            _iconInput.RegisterValueChangedCallback(OnIconChange);
            _placeObjectInput.RegisterValueChangedCallback(OnPlaceObjectChange);
            _sizeInput.RegisterValueChangedCallback(OnSizeChange);
            _stackableInput.RegisterValueChangedCallback(OnStackableChange);
        }

        #endregion

        #region Adding component

        private ObjectField AddObjectFieldToElement(VisualElement parent, int index, string label, Type objectType)
        {
            ObjectField objectField = new ObjectField(label);
            objectField.AddToClassList("input");
            objectField.objectType = objectType;
            parent.Insert(index, objectField);
            return objectField;
        }
        
        private void AddPlaceableItemInput(GroupBox parent)
        {
            GroupBox container = new GroupBox();
            container.AddToClassList("input-with-display");
            ObjectField placeObjectInput = new ObjectField("Place Object");
            Vector2IntField sizeInput = new Vector2IntField("Size");

            placeObjectInput.objectType = typeof(GameObject);

            container.Add(placeObjectInput);
            container.Add(sizeInput);
            
            parent.Add(container);
        }

        #endregion

        #region Utils

        private void ClearGroupBox(GroupBox groupBox)
        {
            groupBox.Clear();
        }
        
        private void RegisterOnSliderChange(EventCallback<ChangeEvent<int>> eventCallback, SliderInt sliderInt)
        {
            sliderInt.RegisterCallback(eventCallback);
        }

        #endregion

        #region OnChange Event

        private void OnItemTypeChange(ChangeEvent<string> eventData)
        {
            string newValue = eventData.newValue;
            GroupBox dynamicInput = rootVisualElement.Q<GroupBox>("DynamicInput");
            ClearGroupBox(dynamicInput);
            switch (newValue)
            {
                case "Item": _currentItemType = "Item"; break;
                case "Placeable Item": AddPlaceableItemInput(dynamicInput);
                    _currentItemType = "Placeable Item"; break;
            }
        }
        
        private void OnBuyPriceChange(ChangeEvent<int> eventData)
        {
            Label displayBuyPrice = rootVisualElement.Q<Label>("BuyPriceDisplay");
            displayBuyPrice.text = eventData.newValue.ToString();
            _currentBuy = eventData.newValue;
        }
        
        private void OnSellPriceChange(ChangeEvent<int> eventData)
        {
            Label displaySellPrice = rootVisualElement.Q<Label>("SellPriceDisplay");
            displaySellPrice.text = eventData.newValue.ToString();
            _currentSell = eventData.newValue;
        }

        private void OnNameChange(ChangeEvent<string> eventData)
        {
            _currentName = eventData.newValue;
        }

        private void OnIconChange(ChangeEvent<Object> eventData)
        {
            if (eventData.newValue is Sprite icon)
            {
                _currentIcon = icon;
            }
            
        }

        private void OnPlaceObjectChange(ChangeEvent<Object> eventData)
        {
            if (eventData.newValue is GameObject gameObject)
            {
                _currentPlaceObject = gameObject;
            }
        }

        private void OnSizeChange(ChangeEvent<Vector2> eventData)
        {
            int x = (int)eventData.newValue.x;
            int y = (int)eventData.newValue.y;
            _currentSize = new Vector2Int(x, y);
        }

        private void OnStackableChange(ChangeEvent<bool> eventData)
        {
            _currenStackable = eventData.newValue;
        }

        #endregion

        private void OnAddItem()
        {
            if (_currentItemType == _itemTypes[0])
            {
                _itemDatabase.AddItem(new Item(
                    0L, _currentIcon, _currentName, _currentBuy, _currentSell, 0
                ));
            }
            Debug.Log(_itemDatabase.GetItemList().Count);
        }
    }
}