using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSnapSystem : MonoBehaviour
{
    public enum ShipTeam
    {
        Blue,
        Red
    };
    
    public enum ShipType
    {
        Atlas,
        Mother,
        Spy
    };
    
    [SerializeField] private GameObject[] blueTeamShipsArray = new GameObject[3];
    [SerializeField] private GameObject[] redTeamShipsArray = new GameObject[3];
    
    private Grid2D<GridObject> _grid;
    private GridObject _currentGridObject;
    
    private ShipTeam _currentTeam;

    private bool _isGrabbed;
    
    private Ship _ship;    
    private List<Ship> _shipGameObjectList;
    private GameObject _shipPrefab;
    private Vector2Int _lastGridPosition;

    public void Setup(Grid2D<GridObject> grid, ShipTeam team)
    {        
        _grid = grid;
        
        _currentTeam = team;

        _isGrabbed = false;
        _shipGameObjectList = new List<Ship>();
        _lastGridPosition = new Vector2Int();
    }
    
    public void Show()
    {
        foreach(Ship ship in _shipGameObjectList)
        {
            ship.Show();
        }
    }

    public void Hide()
    {
        foreach(Ship ship in _shipGameObjectList)
        {
            ship.Hide();
        }
        
        // Destroy anything that's been grabbed
        if (_isGrabbed)
        {
            _isGrabbed = false;
            _ship.Destroy();
        }
    }
    
    // Set the object as grabbed for instantiate when we are over the grid
    public void Grab(ShipType type)
    {
        _isGrabbed = true;
        _shipPrefab = _currentTeam == ShipTeam.Blue ? blueTeamShipsArray[(int)type] : redTeamShipsArray[(int)type];
    }
    
    public bool Deploy()
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
            List<Vector2Int> gridPositionList = _ship.GetGirdOffsetList(_currentGridObject.GetGridPosition());
            
            // Object can be placed on the grid if all object positions are empty
            foreach(Vector2Int position in gridPositionList)
            {
                GridObject gridObject = _grid.GetGridObject(position);
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
                    GridObject gridObject = _grid.GetGridObject(position);
                    gridObject.SetFull(true);
                    gridObject.SetShipReference(_ship.gameObject);
                }
                
                _shipGameObjectList.Add(_ship);
                
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
            GameObject shipGameObject = Instantiate(_shipPrefab, prefabPosition, Quaternion.identity, transform);
            _ship = shipGameObject.GetComponent<Ship>();
        }
        else
        {
            Vector2Int gridPosition = _currentGridObject.GetGridPosition();
            if (!gridPosition.Equals(_lastGridPosition))
            {           
                _ship.Translate(_grid.GetWorldPositionCenter(gridPosition));
                _lastGridPosition = gridPosition;
            }
        }
    }
}