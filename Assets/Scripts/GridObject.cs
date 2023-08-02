using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    public Vector2Int position { get; private set; }
    
    public bool isFull 
    { 
        get { return isFull; }
        set
        {
            isFull = value;
            TriggerUpdateGrid();
        } 
    }
    public bool isOverlayed 
    { 
        get { return isOverlayed; }
        set
        {
            isOverlayed = value;
            TriggerUpdateGrid();
        } 
    }
    public bool isAttacked 
    { 
        get { return isAttacked; }
        set
        {
            isAttacked = value;
            TriggerUpdateGrid();
        }
    }
    public GameObject snappedObject { 
        get { return snappedObject; } 
        set
        {
            snappedObject = value;
            TriggerUpdateGrid();
        }
    }
        
    private Grid2D<GridObject> _grid;
    
    public GridObject(Grid2D<GridObject> grid, int x, int y)
    {
        _grid = grid;
        position = new Vector2Int(x, y);
        
        snappedObject = null;
        
        isFull = false;
        isOverlayed = false;
        isAttacked = false;
    }
    
    public override string ToString()
    {
        return "(" + position.x + "," + position.y + "," + isFull.ToString() + ")";
    }
    
    private void TriggerUpdateGrid()
    {
        _grid.TriggerGridObjectChanged(position);
    }
};