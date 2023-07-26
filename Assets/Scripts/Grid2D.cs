/* 
    Base on Code Monkey Grid System https://www.youtube.com/watch?v=waEsGu--9P8
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/** Example of a GridObject
public class GridObject
{
    private int _x;
    private int _y;
    private Grid<GridObject> _grid;
    
    private bool _status;
    
    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        _x = x;
        _y = y;
        _grid = grid;
        _status = false;
    }
    
    public void SetObjectValue(bool value)
    {
        _status = value;
        _grid.TriggerGridObjectChanged(_x, _y);
    }
    
    public bool GetObjectValue()
    {
        return _status;
    }
    
    public override string ToString()
    {
        return _status.ToString();
    }
};
**/

/** Example of a GridObjectVisual
public class GridObjectVisual : MonoBehaviour
{
    [SerializeField] private Transform _prefab;
    
    private Grid<MapGridObject> _grid;
    private Transform[,] _cellVisual;
    private bool _requiresUpdate = false;

    public void Setup(Grid<MapGridObject> grid)
    {
        _grid = grid;
        _grid.OnGridObjectChanged.AddListener(Grid_OnGridObjectChanged);
        
        _cellVisual = new Transform[_grid.GetWidth(), _grid.GetHeight()];
        
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Vector3 offset = new Vector3(_grid.GetCellSize()/2, _grid.GetCellSize()/2);
                Quaternion rotation = Quaternion.identity;
                
                _cellVisual[x, y] = Instantiate(_prefab, _grid.GetWorldPosition(x, y) + offset, rotation);
            }
        }
        
        UpdateCells();
    } 
    
    private void Update()
    {
        if (_requiresUpdate)
        {
            _requiresUpdate = false;
            UpdateCells();
        }
    }
    
    private void UpdateCells()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                UpdateCell(_cellVisual[x, y],  _grid.GetGridObject(x, y));
            }
        }
    }
    
    private void UpdateCell(Transform cell, MapGridObject gridObject)
    {
       // Method to update when using different visuals
    }
    
    private void Grid_OnGridObjectChanged(Grid<MapGridObject>.OnGridObjectChangedArgs args)
    {
        _requiresUpdate = true;
    }
}
**/

public class Grid2D<TGridObject>
{
    public UnityEvent<OnGridObjectChangedArgs> OnGridObjectChanged;
    public class OnGridObjectChangedArgs {
        public int x;
        public int y;
    };
    
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridObject[,] _cellArray;
    
    private Vector3 _originPosition;
    private TextMesh[,] _debugTextArray;
    
    private bool _isDebugEnabled = false;

    public Grid2D(int width, int height, float cellSize, Vector3 position, Func<Grid2D<TGridObject>, int, int, TGridObject> createObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = position;
    
        OnGridObjectChanged = new UnityEvent<OnGridObjectChangedArgs>();
        
        _cellArray = new TGridObject[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _cellArray[x, y] = createObject(this, x, y);
            }
        }
        
        if (_isDebugEnabled)
        {
            _debugTextArray = new TextMesh[_width, _height];
            
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 centerPosition = GetWorldPosition(x, y) + new Vector3(_cellSize / 2, _cellSize / 2, 0);
                    _debugTextArray[x, y] = CreateDefaultWorldText(_cellArray[x, y].ToString(), centerPosition);
                    
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            
            OnGridObjectChanged.AddListener((OnGridObjectChangedArgs eventArgs) => {
                _debugTextArray[eventArgs.x, eventArgs.y].text = _cellArray[eventArgs.x, eventArgs.y]?.ToString();
            });
        }
    }
    
    public int GetWidth() {
        return _width;
    }

    public int GetHeight() {
        return _height;
    }

    public float GetCellSize() {
        return _cellSize;
    }
    
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }
    
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return GetWorldPosition(gridPosition.x, gridPosition.y);   
    }
    
    public Vector3 GetWorldPositionCenter(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition + new Vector3(_cellSize/2, _cellSize/2);
    }
    
    public Vector3 GetWorldPositionCenter(Vector2Int gridPosition)
    {
        return GetWorldPositionCenter(gridPosition.x, gridPosition.y);   
    }
    
    public void GetGridObjectPosition(Vector3 worldPosition, out int x, out int y)
    {
        Vector2Int gridPosition = GetGridObjectPosition(worldPosition);
        x = gridPosition.x;
        y = gridPosition.y; 
    }
    
    public Vector2Int GetGridObjectPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize), 
            Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize)
        );
    }
    
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return;     
        }
        
        _cellArray[x, y] = value;
        
        TriggerGridObjectChanged(x, y);

        if (_isDebugEnabled)
        {
            _debugTextArray[x, y].text = _cellArray[x, y].ToString();
        }
    }
    
    public void SetGridObject(Vector2Int gridPosition, TGridObject value)
    {
        SetGridObject(gridPosition.x, gridPosition.y, value);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        SetGridObject(GetGridObjectPosition(worldPosition), value);
    }
    
    public TGridObject GetGridObject(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return default(TGridObject);
        }
        
        return _cellArray[x, y];
    }

    public TGridObject GetGridObject(Vector2Int gridPosition)
    {
        return GetGridObject(gridPosition.x, gridPosition.y);
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        return GetGridObject(GetGridObjectPosition(worldPosition));
    }
    
    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged.Invoke(new OnGridObjectChangedArgs { x = x, y = y });
    }
    
    public void TriggerGridObjectChanged(Vector2Int gridPosition)
    {
        TriggerGridObjectChanged(gridPosition.x, gridPosition.y);
    }
    
    private TextMesh CreateWorldText(string text, Vector3 position, int fontSize, TextAlignment alignment, Color color)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        gameObject.transform.position = position;
        
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = alignment;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        
        return textMesh;
    }
    
    private TextMesh CreateDefaultWorldText(string text, Vector3 position)
    {
        return CreateWorldText(text, position, 20, TextAlignment.Center, Color.white);
    }
}
