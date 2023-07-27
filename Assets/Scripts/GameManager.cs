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
    
    [SerializeField] private MapGridVisual _mapGridVisual;
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    
    private Grid2D<MapGridObject> _grid;
    
    private MapGridObject _lastGridObjectOverlayed = null;
    
    private void Start()
    {
        _grid = new Grid2D<MapGridObject>(
            _width,
            _height,
            _cellSize,
            _origin,
            (Grid2D<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y)
        );
        
        _mapGridVisual.Setup(_grid);
        _shipSnapSystem.Setup(_grid);
    }
    
    private void Update()
    {
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
            _shipSnapSystem.Place();
        }
                
        if (Input.GetKeyDown(KeyCode.R))
        {
            _shipSnapSystem.Rotate();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _shipSnapSystem.Grab(_shipPrefabArray[0]);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _shipSnapSystem.Grab(_shipPrefabArray[1]);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _shipSnapSystem.Grab(_shipPrefabArray[2]);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _shipSnapSystem.Grab(_shipPrefabArray[3]);
        }
    }
}
