using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right    
    };
    
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    
    private Direction _direction = Direction.Up;

    public int GetWidth()
    {
        return _width;
    }
    
    public int GetHeight()
    {
        return _height;
    }
    
    public Direction GetDirection()
    {
        return _direction;
    }
    
    public void SetDirection(Direction direction)
    {
        _direction = direction;
        transform.rotation = Quaternion.Euler(0, 0, 90f * (int)_direction);
    }
    
    public Direction GetNextDirection()
    {
        switch (_direction)
        {
            case Direction.Up: return Direction.Left;
            case Direction.Left: return Direction.Down;
            case Direction.Down: return Direction.Right;
            case Direction.Right: return Direction.Up;
        }
        
        return Direction.Up;
    }
    
    public void Rotate()
    {
        _direction = GetNextDirection();
        transform.Rotate(0, 0, 90f);
    }
    
    public void Translate(Vector3 position)
    {
        transform.position = position;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    public List<Vector2Int> GetGridPositionList(Vector2Int offset) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        
        switch (_direction) {
            default:
            case Direction.Up: 
                for (int x = 0; x < _width; x++) {
                    for (int y = 0; y < _height; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Direction.Down:
                for (int x = 0; x < _width; x++) {
                    for (int y = 0; y < _height; y++) {
                        gridPositionList.Add(offset - new Vector2Int(x, y));
                    }
                }
                break;
            case Direction.Left:
                for (int x = 0; x < _height; x++) {
                    for (int y = 0; y < _width; y++) {
                        gridPositionList.Add(offset - new Vector2Int(x, -y));
                    }
                }
                break;
            case Direction.Right:
                for (int x = 0; x < _height; x++) {
                    for (int y = 0; y < _width; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, -y));
                    }
                }
                break;
        }
        
        return gridPositionList;
    }
}
