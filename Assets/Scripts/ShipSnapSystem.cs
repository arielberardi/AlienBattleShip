using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSnapSystem : MonoBehaviour
{
    [SerializeField] private ShipSO _shipSO;
    
    private Grid2D<MapGridObject> _grid;
    private bool _requiresUpdate = false;

    public void Setup(Grid2D<MapGridObject> grid)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
    }
    
    private void Update()
    {   
        if (_requiresUpdate)
        {
            _requiresUpdate = false;
        }
    }
    
    private void Grid_OnGridObjectChanged(Grid2D<MapGridObject>.OnGridObjectChangedArgs args)
    {
        _requiresUpdate = true;
    }
}