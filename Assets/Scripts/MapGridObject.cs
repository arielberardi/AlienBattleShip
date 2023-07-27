using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridObject
{
    private int _x;
    private int _y;
    private Grid2D<MapGridObject> _grid;
    
    private bool _isFull = false;
    private bool _isOverlay = false;
    
    public MapGridObject(Grid2D<MapGridObject> grid, int x, int y)
    {
        _x = x;
        _y = y;
        _grid = grid;
    }
    
    public void SetFull(bool value)
    {
        _isFull = value;
        TriggerUpdateGrid();
    }
    
    public bool GetIsFull()
    {
        return _isFull;
    }
    
    public void SetOverlay(bool value)
    {
        _isOverlay = value;
        TriggerUpdateGrid();
    }
    
    public bool GetIsOverlay()
    {
        return _isOverlay;
    }
    
    public void GetGridPosition(out int x, out int y)
    {
        x = _x;
        y = _y;
    }
    
    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(_x, _y);
    }
    
    public override string ToString()
    {
        return "(" + _x + "," + _y + "," + _isOverlay.ToString() + "," + _isFull.ToString() + ")";
    }
    
    private void TriggerUpdateGrid()
    {
        _grid.TriggerGridObjectChanged(_x, _y);
    }
};