using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttackSystem : MonoBehaviour
{
    private Grid2D<GridObject> _grid;

    public void Setup(Grid2D<GridObject> grid)
    {
        _grid = grid;
    }
    
    public Vector2Int Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GridObject gridObject = _grid.GetGridObject(mousePosition);
        
        // We only update if we are over the grid
        if (gridObject == null)
        {
            return new Vector2Int(-1, -1);
        }
        
        if (gridObject.GetIsAttacked())
        {
            return new Vector2Int(-1, -1);
        }
        
        gridObject.SetAttacked(true);
        return gridObject.GetGridPosition();
    }
    
    public bool Hit(Vector2Int hitPosition)
    {
        GridObject gridObject = _grid.GetGridObject(hitPosition);
        
        if (gridObject.GetIsFull())
        {
            gridObject.Hit();
            return true;
        }
        
        return false;
    }
}
