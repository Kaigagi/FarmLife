using System;
using System.Collections.Generic;
using ItemSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ItemDatabaseWindowLegacy : EditorWindow
    {
        private ItemDatabase _database;

        private readonly List<string> _itemTypes = new List<string>()
        {
            "Item",
            "Placeable Item",
        };

        private readonly List<IMGUIContainer> _frameList = new List<IMGUIContainer>();
        private readonly List<VisualElement> _tabList = new List<VisualElement>();

        [MenuItem("Window/Legacy/Item Database")]
        private static void ShowWindow()
        {
            var window = GetWindow<ItemDatabaseWindowLegacy>("Item Database");
            window.titleContent = new GUIContent("Item Database");
            window.Show();
        }

        private void OnEnable()
        {
            VisualTreeAsset root =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIToolkit Assets/database_window.uxml");
            TemplateContainer container = root.CloneTree();
            rootVisualElement.Add(container);
            
            //Set local variables
            AssignElementsToVariables();
            AddDatabaseInput();
            
            //Adding elements
            VisualElement inputColumn1 = rootVisualElement.Q<VisualElement>("InputColumn1");
            DropdownField itemType = rootVisualElement.Q<DropdownField>("ItemType");
            
            AddObjectFieldToElement(inputColumn1, 1, "Icon", typeof(Texture2D));

            itemType.choices = _itemTypes;
            itemType.RegisterValueChangedCallback(OnItemTypeChange);

            RegisterClickEventToTabs();
        }

        #region Window Elements Setup

        private void AddDatabaseInput()
        {
            VisualElement databaseInput = rootVisualElement.Q<VisualElement>("DatabaseInput");
            ObjectField objectField =
                AddObjectFieldToElement(databaseInput, 0, "Database Instance", typeof(ItemDatabase));
            objectField.AddToClassList("single-line-input");
        }

        /**
         * Assign some major element into local variables
         */
        private void AssignElementsToVariables()
        {
            _frameList.Add(rootVisualElement.Q<IMGUIContainer>("AddFrame"));
            _frameList.Add(rootVisualElement.Q<IMGUIContainer>("RemoveFrame"));
            _frameList.Add(rootVisualElement.Q<IMGUIContainer>("FindFrame"));

            _tabList.Add(rootVisualElement.Q<VisualElement>("AddTab"));
            _tabList.Add(rootVisualElement.Q<VisualElement>("RemoveTab"));
            _tabList.Add(rootVisualElement.Q<VisualElement>("FindTab"));
        }

        private void RegisterClickEventToTabs()
        {
            VisualElement addTab = rootVisualElement.Q<VisualElement>("AddTab");
            VisualElement removeTab = rootVisualElement.Q<VisualElement>("RemoveTab");
            VisualElement findTab = rootVisualElement.Q<VisualElement>("FindTab");

            addTab.RegisterCallback<ClickEvent>(OnAddTab);
            removeTab.RegisterCallback<ClickEvent>(OnRemoveTab);
            findTab.RegisterCallback<ClickEvent>(OnFindTab);
        }

        private void OnAddTab(ClickEvent eventData)
        {
            SwitchFrame(_frameList[0]);
        }

        private void OnRemoveTab(ClickEvent eventData)
        {
            SwitchFrame(_frameList[1]);
        }

        private void OnFindTab(ClickEvent eventData)
        {
            SwitchFrame(_frameList[2]);
        }

        private void SwitchFrame(IMGUIContainer frame)
        {
            for (var index = 0; index < _frameList.Count; index++)
            {
                var fr = _frameList[index];
                if (fr == frame)
                {
                    fr.style.display = DisplayStyle.Flex;
                    _tabList[index].RemoveFromClassList("tab");
                    _tabList[index].AddToClassList("selected-tab");
                    continue;
                }

                fr.style.display = DisplayStyle.None;

                _tabList[index].RemoveFromClassList("selected-tab");
                _tabList[index].AddToClassList("tab");
            }
        }

        #endregion

        private ObjectField AddObjectFieldToElement(VisualElement parent, int index, string label, Type objectType)
        {
            ObjectField objectField = new ObjectField(label);
            objectField.AddToClassList("input");
            objectField.objectType = objectType;
            parent.Insert(index, objectField);
            return objectField;
        }

        private void AddInputStyle(VisualElement element)
        {
            element.AddToClassList("input");
        }

        #region Add Item Frame

        private void OnItemTypeChange(ChangeEvent<string> eventData)
        {
            string newValue = eventData.newValue;
            GroupBox dynamicInput = rootVisualElement.Q<GroupBox>("DynamicInput");
            ClearGroupBox(dynamicInput);
            switch (newValue)
            {
                case "Item": AddItemInput(dynamicInput); break;
                case "Placeable Item": AddPlaceableItemInput(dynamicInput); break;
                default: AddItemInput(dynamicInput); break;
            }
        }

        private void AddItemInput(GroupBox parent)
        {
            AddSliderIntWithValueDisplay("DisplayBuyPrice", "Buy Price", OnBuyPriceChange, parent);
            AddSliderIntWithValueDisplay("DisplaySellPrice", "Sell Price", OnSellPriceChange, parent);
        }

        private void AddSliderIntWithValueDisplay(string labelName, string inputLabel,
            EventCallback<ChangeEvent<int>> callback, GroupBox parent)
        {
            GroupBox container = new GroupBox();
            container.AddToClassList("input-with-display");
            Label display = new Label();
            display.name = labelName;
            display.style.marginLeft = 3;
            display.style.marginRight = 3;
            display.style.marginTop = 3;
            SliderInt priceInput = new SliderInt(inputLabel, 0, 9999);
            priceInput.RegisterValueChangedCallback(callback);
            container.Add(priceInput);
            container.Add(display);

            priceInput.AddToClassList("input-slider");
            parent.Add(container);
        }
        

        private void OnBuyPriceChange(ChangeEvent<int> eventData)
        {
            Label displayBuyPrice = rootVisualElement.Q<Label>("DisplayBuyPrice");
            displayBuyPrice.text = eventData.newValue.ToString();
        }
        
        private void OnSellPriceChange(ChangeEvent<int> eventData)
        {
            Label displaySellPrice = rootVisualElement.Q<Label>("sellPriceInput");
            displaySellPrice.text = eventData.newValue.ToString();
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

            AddItemInput(parent);
        }

        #endregion

        private void ClearGroupBox(GroupBox groupBox)
        {
            groupBox.Clear();
        }
    }
}