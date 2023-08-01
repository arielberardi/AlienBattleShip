using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridVisual : MonoBehaviour
{
    public enum MapType 
    {
        Place,
        Attack
    }
    
    [SerializeField] private GameObject _cellPrefab;
    
    private Grid2D<GridObject> _grid;
    private GameObject[,] _cellVisualArray;
    private bool _requiresUpdate;

    private GridObject _lastGridObjectOverlayed;
    
    private MapType _mapType;

    public void Setup(Grid2D<GridObject> grid, MapType type)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _mapType = type;
        
        _requiresUpdate = false;
        
        _cellVisualArray = new GameObject[_grid.GetWidth(), _grid.GetHeight()];
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3 offset = new Vector3(_grid.GetCellSize()/2, _grid.GetCellSize()/2);
                Quaternion rotation = Quaternion.identity;
                
                _cellVisualArray[x, y] = Instantiate(_cellPrefab, _grid.GetWorldPosition(x, y) + offset, rotation, transform);
                UpdateCell(_cellVisualArray[x, y],  _grid.GetGridObject(x, y));
            }
        }
    } 

    public void Update()
    {
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
    
    private void Grid_OnGridObjectChanged(Grid2D<GridObject>.OnGridObjectChangedArgs args)
    {
        UpdateCell(_cellVisualArray[args.x, args.y],  _grid.GetGridObject(args.x, args.y));
    }
    
    private void UpdateCell(GameObject cell, GridObject gridObject)
    {
        GridObjectVisual gridVisual = cell.GetComponent<GridObjectVisual>();
        
        if (_mapType == MapType.Attack)
        {
            gridVisual.SetSelected(gridObject.GetIsOverlay() || gridObject.GetIsAttacked());
            gridVisual.SetAttacked(gridObject.GetIsAttacked());
        }        
        else
        {
            gridVisual.SetSelected(gridObject.GetIsOverlay() || gridObject.GetIsFull());
        }
    }
    
    private void UpdateOverlayStatus()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
    
        if (gridObjectOverlayed == null)
        {
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = null;
            return;
        }
        
        // Do not add overlay to an already selected grid
        if (gridObjectOverlayed != null)
        {
            if (_mapType == MapType.Attack &&  gridObjectOverlayed.GetIsAttacked())
            {
                return;
            }
            
            if (_mapType == MapType.Place && gridObjectOverlayed.GetIsFull())
            {
                return;
            }
        }   
   
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            gridObjectOverlayed.SetOverlay(true);
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
    }
}