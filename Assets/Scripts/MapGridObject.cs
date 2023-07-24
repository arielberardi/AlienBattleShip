using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridObject
{
    private int _x;
    private int _y;
    private Grid2D<MapGridObject> _grid;
    
    private bool _isEmpty = false;
    
    public MapGridObject(Grid2D<MapGridObject> grid, int x, int y)
    {
        _x = x;
        _y = y;
        _grid = grid;
    }
    
    private void SetEmpty(bool value)
    {
        _isEmpty = value;
    }
    
    private bool GetIsEmpty()
    {
        return _isEmpty;
    }
    
    public override string ToString()
    {
        return "(" + _x + "," + _y + "," + _isEmpty.ToString() + ")";
    }
};