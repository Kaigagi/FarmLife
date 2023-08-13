using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

public class PlacementSystem : MonoBehaviour, IStateAdapter
{
    public BuildingList buildingList;
    public Material previewMaterialPrefab;
    public MapData mapData;

    private Grid _grid;
    private Vector3 _lastPosition;

    public GameObject mouseIndicator;
    private GameObject _buildingInstance;
    private Material _previewMaterialInstance;
    private bool _isPlaceAvailable = true;
    private PlaceableObject _currentBuilding;

    [SerializeField] private Inventory inventory;
    private QuickItemBar _quickItemBar;
    
    private GameObject BuildingInstance
    {
        get => _buildingInstance;
        set
        {
            if (value == null)
            {
                return;
            }

            _buildingInstance = value;
        }
    }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        
        _previewMaterialInstance = previewMaterialPrefab;
        // _currentBuilding = buildingList.list[0];
        mapData.BuildingList1.Clear();
    }

    private void Start()
    {
        _quickItemBar = inventory.QuickItemBar;
        _quickItemBar.OnChangeSelectItem += PlaceBuildingThroughQuickAccess;
    }

    private void PlaceBuildingThroughQuickAccess(int position)
    {
        Item stackableItem = _quickItemBar.SelectItem(position);
        if (stackableItem == null || stackableItem is PlaceableObject)
        {
            return;
        }

        
    }


    #region Testing Blocks

    //For testing
    // private void Update()
    // {
    //     mouseIndicator.transform.position = GetMousePositionWorld();
    // }

    #endregion

    #region State Adapter Implement

    public void StateSetup()
    {
        _currentBuilding = buildingList.list[0];
        Cursor.lockState = CursorLockMode.None;
        _buildingInstance = Instantiate(_currentBuilding.placeObject, Vector3.zero, Quaternion.identity);
    }

    public void StatefulUpdate()
    {
        Vector3 mousePos = GetMousePositionWorld();
        _buildingInstance.transform.position = SnapObjectIntoGrid(mousePos);
        ApplyColorToPlaceholder();
        PreparePreview();
        CheckAvailability(mousePos);
    }

    public void StateCleanup()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(_buildingInstance.gameObject);
    }

    #endregion

    /**
     * Place building at placeholder position
     */
    public void PlaceBuilding()
    {
        if (!_isPlaceAvailable) return;
        
        Vector3 position = _buildingInstance.transform.position;
        GameObject newBuilding = Instantiate(_currentBuilding.placeObject, position, Quaternion.identity);
        
        SaveToMapData(_currentBuilding, _grid.WorldToCell(position));
    }

    private void CheckAvailability(Vector3 mousePosition)
    {
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        Vector3Int[] estimatedOccupyingPosition = _currentBuilding.CalculateTile(gridPosition);
        foreach (var position in estimatedOccupyingPosition)
        {
            if (mapData.BuildingList1.ContainsKey(position))
            {
                _isPlaceAvailable = false;
                return;
            }
        }

        _isPlaceAvailable = true;
    }
    
    private void PreparePreview()
    {
        Renderer[] renderers = _buildingInstance.GetComponentsInChildren<Renderer>();
        foreach (var childrenRenderer in renderers)
        {
            Material[] materials = childrenRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _previewMaterialInstance;
            }

            childrenRenderer.materials = materials;
        }
    }

    private void ApplyColorToPlaceholder()
    {
        if (_isPlaceAvailable)
        {
            _previewMaterialInstance.SetColor("_Color", new Color(0.4f, 1f, 0.44f, 0.6f));
        }
        else
        {
            _previewMaterialInstance.SetColor("_Color", new Color(0.96f, 0.38f, 0.33f, 0.6f));
        }
    }

    #region Utils

    public Vector3 SnapObjectIntoGrid(Vector3 position)
    {
        Vector3Int cellPos = _grid.WorldToCell(position);
        return _grid.CellToWorld(cellPos);
    }

    public Vector3 GetMousePositionWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            _lastPosition = hitPoint.point;
        }

        return _lastPosition;
    }

    public void SaveToMapData(PlaceableObject building, Vector3Int startPosition)
    {
        Vector3Int[] tiles = building.CalculateTile(startPosition);
        foreach (var tile in tiles)
        {
            mapData.BuildingList1.Add(tile, building);
        }
    }

    #endregion
}
