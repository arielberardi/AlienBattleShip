using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    
    [SerializeField, Space] private GameObject _gridObjectVisual;
    
    private Grid2D<GridObject> _grid;
    private GameObject[,] _gridVisualArray;

    private GridObject _lastGridObjectOverlayed;
    
    public void Hide()
    {
        for (int x = 0; x < _grid.width; x++)
        {
            for (int y = 0; y < _grid.height; y++)
            {
                _gridVisualArray[x, y].SetActive(false);
            }
        }
    }
    
    public void Show()
    {
        for (int x = 0; x < _grid.width; x++)
        {
            for (int y = 0; y < _grid.height; y++)
            {
                _gridVisualArray[x, y].SetActive(true);
            }
        }
    }
    
    private void Start()
    {
        _grid = new Grid2D<GridObject>(
            _width, 
            _height, 
            _cellSize, 
            transform.position,
            (Grid2D<GridObject> g, int x, int y) => new GridObject(g, x, y)
        );
        
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _gridVisualArray = new GameObject[_grid.width, _grid.height];
        for (int x = 0; x < _grid.width; x++)
        {
            for (int y = 0; y < _grid.height; y++)
            {
                Vector3 position = _grid.GridToWorldPositionCenter(x, y);
                _gridVisualArray[x, y] = Instantiate(_gridObjectVisual, position, Quaternion.identity, transform);
            }
        }
    } 

    private void Update()
    {
        UpdateOverlayStatus();
    }
    
    private void Grid_OnGridObjectChanged(Grid2D<GridObject>.OnGridObjectChangedArgs args)
    {
        UpdateCell(_gridVisualArray[args.x, args.y],  _grid.GetGridObject(args.x, args.y));
    }
    
    private void UpdateCell(GameObject cell, GridObject gridObject)
    {
        GridObjectVisual gridVisual = cell.GetComponent<GridObjectVisual>();
        gridVisual.isSelected = gridObject.isOverlayed;
    }
    
    private void UpdateOverlayStatus()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
    
        if (gridObjectOverlayed == null)
        {
            if (_lastGridObjectOverlayed != null)
            {
                _lastGridObjectOverlayed.isOverlayed = false;
            }
            
            _lastGridObjectOverlayed = null;
            return;
        }
        
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            if (_lastGridObjectOverlayed != null)
            {
                _lastGridObjectOverlayed.isOverlayed = false;
            }
            
            gridObjectOverlayed.isOverlayed = true;
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
    }
}