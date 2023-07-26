using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _origin;
    [SerializeField] private GameObject _shipPrefab;
    
    [SerializeField] private MapGridVisual _mapGridVisual;
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    
    private Grid2D<MapGridObject> _grid;
    
    private MapGridObject _lastGridObjectOverlayed = null;
    
    // Start is called before the first frame update
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
    
    // Update is called once per frame
    private void Update()
    {
        UpdateOverlayedCell();
        UpdateShipPosition();
    }
    
    
    // TODO: Refactor so it can match the size of current ship and improve shaders to make the 
    // grid sprite luminated
    // It may need all toghether in the ShipSnapSystem
    private void UpdateOverlayedCell()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MapGridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
   
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            gridObjectOverlayed.SetOverlay(true);
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
        
        if (gridObjectOverlayed == null)
        {
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = null;
        }
    }
    
    private void UpdateShipPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _shipSnapSystem.Grab(_shipPrefab);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _shipSnapSystem.Place();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _shipSnapSystem.Rotate();
        }
    }
}
