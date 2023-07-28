using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    
    private Grid2D<MapGridObject> _grid;
    private GameObject[,] _cellVisualArray;
    private bool _requiresUpdate;

    private MapGridObject _lastGridObjectOverlayed;

    public void Setup(Grid2D<MapGridObject> grid)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _requiresUpdate = false;
        
        _cellVisualArray = new GameObject[_grid.GetWidth(), _grid.GetHeight()];
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3 offset = new Vector3(_grid.GetCellSize()/2, _grid.GetCellSize()/2);
                Quaternion rotation = Quaternion.identity;
                
                _cellVisualArray[x, y] = Instantiate(_prefab, _grid.GetWorldPosition(x, y) + offset, rotation, transform);
            }
        }
        
        UpdateCells();
    } 

    public void Update()
    {
        if (_requiresUpdate)
        {
            _requiresUpdate = false;
            UpdateCells();
        }
        
        UpdateOverlayStatus();
    }
    
    public void Hide()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _cellVisualArray[x, y].SetActive(false);
            }
        }
    }
    
    public void Show()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _cellVisualArray[x, y].SetActive(true);
            }
        }
    }
    
    private void Grid_OnGridObjectChanged(Grid2D<MapGridObject>.OnGridObjectChangedArgs args)
    {
        _requiresUpdate = true;
    }
    
    private void UpdateCells()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                UpdateCell(_cellVisualArray[x, y],  _grid.GetGridObject(x, y));
            }
        }
    }
    
    private void UpdateCell(GameObject cell, MapGridObject gridObject)
    {
        cell.GetComponent<GridCellVisual>().SetSelected(gridObject.GetIsOverlay() || gridObject.GetIsFull());
    }
    
    private void UpdateOverlayStatus()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MapGridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
        
        if (gridObjectOverlayed == null)
        {
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = null;
            return;
        }
        
        if (gridObjectOverlayed != null && gridObjectOverlayed.GetIsFull())
        {
            return;
        }   
   
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            gridObjectOverlayed.SetOverlay(true);
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
    }
}