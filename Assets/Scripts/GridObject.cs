using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private int _x;
    private int _y;
    private Grid2D<GridObject> _grid;
    private Ship _ship;
    
    private bool _isFull;
    private bool _isOverlay;
    private bool _isAttacked;
    
    public GridObject(Grid2D<GridObject> grid, int x, int y)
    {
        _x = x;
        _y = y;
        _grid = grid;
        
        _ship = null;
        
        _isFull = false;
        _isOverlay = false;
        _isAttacked = false;
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
    
    public void SetShipReference(GameObject ship)
    {
        _ship = ship.GetComponent<Ship>();
    }
    
    public void Hit()
    {
        _ship.Damage();
    }
    
    public void SetAttacked(bool value)
    {
        _isAttacked = value;
        TriggerUpdateGrid();
    }
    
    public bool GetIsAttacked()
    {
        return _isAttacked;
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