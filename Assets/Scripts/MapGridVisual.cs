using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridVisual : MonoBehaviour
{
    // [SerializeField] private Transform _prefab;
    
    private Grid2D<MapGridObject> _grid;
    private Transform[,] _cellVisual;
    private bool _requiresUpdate = false;

    public void Setup(Grid2D<MapGridObject> grid)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _cellVisual = new Transform[_grid.GetWidth(), _grid.GetHeight()];
        
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3 offset = new Vector3(_grid.GetCellSize()/2, _grid.GetCellSize()/2);
                Quaternion rotation = Quaternion.identity;
                
                // _cellVisual[x, y] = Instantiate(_prefab, _grid.GetWorldPosition(x, y) + offset, rotation);
            }
        }
        
        UpdateCells();
    } 
    
    private void Update()
    {
        if (_requiresUpdate)
        {
            _requiresUpdate = false;
            UpdateCells();
        }
    }
    
    private void UpdateCells()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                UpdateCell(_cellVisual[x, y],  _grid.GetGridObject(x, y));
            }
        }
    }
    
    private void UpdateCell(Transform cell, MapGridObject gridObject)
    {
       // Method to update when using different visuals
    }
    
    private void Grid_OnGridObjectChanged(Grid2D<MapGridObject>.OnGridObjectChangedArgs args)
    {
        _requiresUpdate = true;
    }
}