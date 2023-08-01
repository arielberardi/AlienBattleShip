using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _gridObjectVisual;
    
    private Grid2D<GridObject> _grid;
    private GameObject[,] _gridVisualArray;
    private bool _requiresUpdate;

    private GridObject _lastGridObjectOverlayed;

    public void Setup(Grid2D<GridObject> grid)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _gridVisualArray = new GameObject[_grid.GetWidth(), _grid.GetHeight()];
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3 position = _grid.GetWorldPosition(x, y) + new Vector3(_grid.GetCellSize()/2, _grid.GetCellSize()/2);
                _gridVisualArray[x, y] = Instantiate(_gridObjectVisual, position, Quaternion.identity, transform);
            }
        }
    } 

    public void Update()
    {
        UpdateOverlayStatus();
    }
    
    public void Hide()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _gridVisualArray[x, y].SetActive(false);
            }
        }
    }
    
    public void Show()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _gridVisualArray[x, y].SetActive(true);
            }
        }
    }
    
    private void Grid_OnGridObjectChanged(Grid2D<GridObject>.OnGridObjectChangedArgs args)
    {
        UpdateCell(_gridVisualArray[args.x, args.y],  _grid.GetGridObject(args.x, args.y));
    }
    
    private void UpdateCell(GameObject cell, GridObject gridObject)
    {
        GridObjectVisual gridVisual = cell.GetComponent<GridObjectVisual>();
        gridVisual.SetSelected(gridObject.GetIsOverlay() || gridObject.GetIsFull() || gridObject.GetIsAttacked());
    }
    
    private void UpdateOverlayStatus()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GridObject gridObjectOverlayed = _grid.GetGridObject(mousePosition);
    
        if (gridObjectOverlayed == null)
        {
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = null;
            return;
        }
        
        // Do not add overlay to an already selected grid
        if (gridObjectOverlayed != null)
        {
            if (gridObjectOverlayed.GetIsFull() || gridObjectOverlayed.GetIsAttacked())
            {
                return;
            }
        }
   
        if (gridObjectOverlayed != null && _lastGridObjectOverlayed != gridObjectOverlayed)
        {
            gridObjectOverlayed.SetOverlay(true);
            _lastGridObjectOverlayed?.SetOverlay(false);
            _lastGridObjectOverlayed = gridObjectOverlayed;
        }
    }
}