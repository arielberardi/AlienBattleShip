using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Will make ax list of this ships so GameManager just need to select a SHIP TYPE
public class ShipSnapSystem : MonoBehaviour
{
    private Grid2D<MapGridObject> _grid;
    private MapGridObject _currentGridObject;
    
    private bool _isGrabbed = false;
    
    private GameObject _shipPrefab = null;
    private Ship _ship = null;
    
    private Vector2Int _lastGridPosition = new Vector2Int();

    public void Setup(Grid2D<MapGridObject> grid)
    {
        _grid = grid;
    }
    
    // Set the object as grabbed for instantiate when we are over the grid
    public void Grab(GameObject prefab)
    {
        _isGrabbed = true;
        _shipPrefab = prefab;
    }
    
    public bool Place()
    {
        // Place only if we grab the object and is instantiated
        if (!_isGrabbed || _ship == null) 
        {
            return false;
        }
        
        // Destroy object if we are trying to place outisde the grid
        if (_currentGridObject == null)
        {
            _ship.Destroy();
            _isGrabbed = false;
            return true;
        }
        else
        {
            bool canBePlaced = true;
            List<Vector2Int> gridPositionList = _ship.GetGridPositionList(_currentGridObject.GetGridPosition());
            
            // Object can be placed on the grid if all object positions are empty
            foreach(Vector2Int position in gridPositionList)
            {
                MapGridObject gridObject = _grid.GetGridObject(position);
                if (gridObject == null || gridObject.GetIsFull())
                {
                    canBePlaced = false;
                    break;
                }
            }
            
            // Set the grid as full and keep the object in place
            if (canBePlaced)
            {
                foreach(Vector2Int position in gridPositionList)
                {   
                    MapGridObject gridObject = _grid.GetGridObject(position);
                    
                    if (gridObject != null)
                    {
                        gridObject.SetFull(true);
                        gridObject.SetOverlay(true);
                    }
                }
                
                _ship = null;
                _isGrabbed = false;
                return true;
            }
        }
        
        return false;
    }
    
    public bool Rotate()
    {
        // Rotate only if we grab the object and is instantiated
        if (!_isGrabbed || _ship == null) 
        {
            return false;
        }
        
        _ship.Rotate();
        return true;
    }
    
    private void Update()
    {   
        // We only update when we have a ship
        if (!_isGrabbed)
        {
            return;
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 prefabPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        _currentGridObject = _grid.GetGridObject(prefabPosition);
        
        // We only update if we are over the grid
        if (_currentGridObject == null)
        {
            return;
        }
        
        // Create Prefab if doesn't exist before, move it if exist
        if (_ship == null)
        {
            _ship = Instantiate(_shipPrefab, prefabPosition, Quaternion.identity).GetComponent<Ship>();
        }
        else
        {
            Vector2Int gridPosition = _currentGridObject.GetGridPosition();
            if (!gridPosition.Equals(_lastGridPosition))
            {           
                // TODO: Add a lerp to the movement
                _ship.Translate(_grid.GetWorldPositionCenter(gridPosition));
                _lastGridPosition = gridPosition;
            }
        }
    }
}