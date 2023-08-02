/* 
    Base on Code Monkey Grid System https://www.youtube.com/watch?v=waEsGu--9P8
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Grid2D<TGridObject>
{
    public UnityEvent<OnGridObjectChangedArgs> OnGridObjectChanged;
    public class OnGridObjectChangedArgs {
        public int x;
        public int y;
    };
    
    public int width { get; private set; }
    public int height { get; private set; }
    public float cellSize { get; private set; }
    
    private TGridObject[,] _cellArray;
    private Vector3 _originPosition;
    private TextMesh[,] _debugTextArray;
    
    private bool _isDebugEnabled; // Enable to preview the grid on the screen using Debug.DrawLine and Text

    // This constructor is expected to received a function of the TGridObject to use 
    // Signature: TGridObject (Grid2D<TGridObject> g, int x, int y)
    public Grid2D(int w, int h, float cellsize, Vector3 position, 
                  Func<Grid2D<TGridObject>, int, int, TGridObject> createObject)
    {
        width = width;
        height = height;
        cellSize = cellsize;
        _originPosition = position;
    
        OnGridObjectChanged = new UnityEvent<OnGridObjectChangedArgs>();
        
        _cellArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _cellArray[x, y] = createObject(this, x, y);
            }
        }
        
        _isDebugEnabled = false;
        
        // This is only for debug purposes
        // It will draw lines and text for each cell
        if (_isDebugEnabled)
        {
            _debugTextArray = new TextMesh[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 centerPosition = GridToWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2, 0);
                    _debugTextArray[x, y] = CreateDefaultWorldText(_cellArray[x, y].ToString(), centerPosition);
                    
                    Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            
            Debug.DrawLine(GridToWorldPosition(0, height), GridToWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GridToWorldPosition(width, 0), GridToWorldPosition(width, height), Color.white, 100f);
            
            OnGridObjectChanged.AddListener((OnGridObjectChangedArgs eventArgs) => {
                _debugTextArray[eventArgs.x, eventArgs.y].text = _cellArray[eventArgs.x, eventArgs.y]?.ToString();
            });
        }
    }
    
    public Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + _originPosition;
    }
    
    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return GridToWorldPosition(gridPosition.x, gridPosition.y);   
    }
    
    public Vector3 GridToWorldPositionCenter(int x, int y)
    {
        return new Vector3(x, y) * cellSize + _originPosition + new Vector3(cellSize / 2, cellSize / 2);
    }
    
    public Vector3 GridToWorldPositionCenter(Vector2Int gridPosition)
    {
        return GridToWorldPositionCenter(gridPosition.x, gridPosition.y);   
    }
    
    public void WolrdToGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        Vector2Int gridPosition = WolrdToGridPosition(worldPosition);
        x = gridPosition.x;
        y = gridPosition.y; 
    }
    
    public Vector2Int WolrdToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition - _originPosition).x / cellSize), 
            Mathf.FloorToInt((worldPosition - _originPosition).y / cellSize)
        );
    }
    
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;     
        }
        
        _cellArray[x, y] = value;
        
        if (_isDebugEnabled)
        {
            _debugTextArray[x, y].text = _cellArray[x, y].ToString();
        }
        
        TriggerGridObjectChanged(x, y);
    }
    
    public void SetGridObject(Vector2Int gridPosition, TGridObject value)
    {
        SetGridObject(gridPosition.x, gridPosition.y, value);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        SetGridObject(WolrdToGridPosition(worldPosition), value);
    }
    
    public TGridObject GetGridObject(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
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
        return GetGridObject(WolrdToGridPosition(worldPosition));
    }
    
    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged.Invoke(new OnGridObjectChangedArgs { x = x, y = y });
    }
    
    public void TriggerGridObjectChanged(Vector2Int gridPosition)
    {
        TriggerGridObjectChanged(gridPosition.x, gridPosition.y);
    }
    
    // Create World Text on the world space just for debug purposes
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
