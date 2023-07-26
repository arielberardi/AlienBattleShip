using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _origin;
    [SerializeField] private ShipSO _shipSO;
    
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
    
    
    // TODO:
    // Refactor removes SO and uses prefab instead with generic script
    // Refactor Rotation/Direction forward of with and height
        // Rotation will be handled by the prefab
        // Direction will be returned by the prefab (make no senses to keep using a SO)
    // Refactor Height/Width calculation to gridplaces
        // Depending on direction will calculate which places the object will fill
        // We update those fill places or we return if object can't be placed
        // This will be a logic inside the ShipSnapSystem
        // GameManager will pass the expected object to be used
        
    GameObject ship = null;
    bool objectGrabbed = false;
    int rotationCount = 1;

    // Update is called once per frame
    private void Update()
    {
        UpdateOverlayedCell();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 prefabPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        
        if (!objectGrabbed && Input.GetMouseButtonDown(0))
        {
            ship = Instantiate(_shipSO.prefab, prefabPosition, Quaternion.identity);
            objectGrabbed = true;
        }
        
        if (ship != null && objectGrabbed)
        {
            int x, y;
            _grid.GetGridObjectPosition(prefabPosition, out x, out y);
            MapGridObject currentGridObject = _grid.GetGridObject(x, y);
            
            if (currentGridObject != null)
            {
                ship.transform.position = _grid.GetWorldPositionCenter(x, y);
            }   
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ship.transform.Rotate(0, 0, 90f);
            }

            if (Input.GetMouseButtonDown(1))
            {                
                if (currentGridObject == null)
                {
                    Destroy(ship);
                    objectGrabbed = false;
                }
                else
                {
                    bool canBePlaced = true;
                    
                    // The check of posible grids must be done depending on current forward Direction
                    for (int i = 0; i < _shipSO.height; i++)
                    {
                        MapGridObject nextGridObject = _grid.GetGridObject(x, y + i);
                        if (nextGridObject == null || nextGridObject.GetIsFull())
                        {
                            canBePlaced = false;
                        }
                    }

                    if (canBePlaced)
                    {
                        objectGrabbed = false;
                        
                        for (int i = 0; i < _shipSO.height; i++)
                        {
                            _grid.GetGridObject(x, y + i).SetFull(true);
                        }
                        
                        Debug.Log("New position full: (" + x + "," + y + ")");
                    }
                    else
                    {
                        Debug.Log("Grid position full or null: (" + x + "," + y + ")");
                    }
                }
            }
        }
    }
    
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
}
