using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private int[,] _gridCells;
    
    private TextMesh[,] _debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridCells = new int[_width, _height];
        
        _debugTextArray = new TextMesh[_width, _height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Assign _gridCells[x, y] to an object
                
                Vector3 textPosition = GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
                _debugTextArray[x, y] = CreateWorldText(_gridCells[x, y].ToString(), null, textPosition, Color.white);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
            
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
    
    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || x > _width || y < 0 || y > _height)
        {
            return;
        }
        
        _gridCells[x, y] = value;
        _debugTextArray[x, y].text = value.ToString();
    }
    
    public void SetValue(Vector3 worldPosition, int value)
    {
        Vector2 gridPosition = GetGridPosition(worldPosition);
        SetValue((int)gridPosition.x, (int)gridPosition.y, value);
    }
    
    public int GetValue(int x, int y)
    {
        if (x < 0 || x > _width || y < 0 || y > _height)
        {
            return 0;
        }
        
        return _gridCells[x, y];
    }
    
    public int GetValue(Vector3 worldPosition)
    {
        Vector2 gridPosition = GetGridPosition(worldPosition);
        return GetValue((int)gridPosition.x, (int)gridPosition.y);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }
    
    private Vector2 GetGridPosition(Vector3 position) 
    {
        return new Vector2(
            Mathf.FloorToInt((position - _originPosition).x / _cellSize),
            Mathf.FloorToInt((position - _originPosition).y / _cellSize)
        );
    }
    
    private TextMesh CreateWorldText(string text, Transform parent, Vector3 position, Color color)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.position = position;
        
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.fontSize = 40;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.text = text;
        
        return textMesh;
    }
}  
