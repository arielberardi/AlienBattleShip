using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttackSystem : MonoBehaviour
{
    private Grid2D<MapGridObject> _grid;
    private List<GameObject> _attackedPointList;

    public void Setup(Grid2D<MapGridObject> grid)
    {
        _grid = grid;
        _attackedPointList = new List<GameObject>();
    }
    
    public void Show()
    {
        foreach(GameObject attackedPoint in _attackedPointList)
        {
            attackedPoint.SetActive(true);
        }
    }
    
    public void Hide()
    {
        foreach(GameObject attackedPoint in _attackedPointList)
        {
            attackedPoint.SetActive(false);
        }
    }
    
    public bool Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MapGridObject gridObject = _grid.GetGridObject(mousePosition);
        
        // We only update if we are over the grid
        if (gridObject == null)
        {
            return false;
        }
        
        if (gridObject.GetIsAttacked())
        {
            return false;
        }
                
        gridObject.SetAttacked(true);
        return true;
    }
}
