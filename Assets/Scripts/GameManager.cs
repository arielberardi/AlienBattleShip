using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _origin;
    [SerializeField] private GameObject[] _shipPrefabArray;
    
    [SerializeField] private MapGridVisual[] _mapGridVisual;
    [SerializeField] private ShipSnapSystem[] _shipSnapSystem;
    
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;
    
    private Grid2D<MapGridObject> _grid;
    private MapGridObject _lastGridObjectOverlayed;
    private int _currentPlayer;
    
    private void Start()
    {
        _grid = new Grid2D<MapGridObject>(
            _width,
            _height,
            _cellSize,
            _origin,
            (Grid2D<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y)
        );
        
        _currentPlayer = 0;
        
        _mapGridVisual[0].Setup(_grid);
        _shipSnapSystem[0].Setup(_grid, _player1);
        _mapGridVisual[0].Show();
        _shipSnapSystem[0].Show();
    
        _mapGridVisual[1].Setup(_grid);
        _shipSnapSystem[1].Setup(_grid, _player2);
        _mapGridVisual[1].Hide();
        _shipSnapSystem[1].Hide();
    }
    
    private void Update()
    {
        _mapGridVisual[_currentPlayer].Update();

        UpdateOverlayedCell();
        UpdateShipPosition();
    }
    
    private void UpdateOverlayedCell()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MapGridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
        
        if (gridObjectOverlayed == null)
        {
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = null;
            return;
        }
        
        if (gridObjectOverlayed != null && gridObjectOverlayed.GetIsFull())
        {
            return;
        }   
   
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            gridObjectOverlayed.SetOverlay(true);
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
    }
    
    private void UpdateShipPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _shipSnapSystem[_currentPlayer].Place();
        }
                
        if (Input.GetKeyDown(KeyCode.R))
        {
            _shipSnapSystem[_currentPlayer].Rotate();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _shipSnapSystem[_currentPlayer].Grab(_shipPrefabArray[0]);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _shipSnapSystem[_currentPlayer].Grab(_shipPrefabArray[1]);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _shipSnapSystem[_currentPlayer].Grab(_shipPrefabArray[2]);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _shipSnapSystem[_currentPlayer].Grab(_shipPrefabArray[3]);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _mapGridVisual[_currentPlayer].Hide();
            _shipSnapSystem[_currentPlayer].Hide();
            
            _currentPlayer = _currentPlayer == 1 ? 0 : 1;
            
            _mapGridVisual[_currentPlayer].Show();
            _shipSnapSystem[_currentPlayer].Show();
        }
    }
}
