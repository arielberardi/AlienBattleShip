using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    public UnityEvent<OnShipSinkedArgs> OnShipSinked;
    public class OnShipSinkedArgs {
        public GameObject ship;
    }
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right    
    };
    
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _health;
    
    private Vector3 _targetPosition;
    private Direction _direction;
    
    private int _currentHealth;

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
        _targetPosition = position;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Damage()
    {
        if (_currentHealth == 0)
        {
            OnShipSinked.Invoke(new OnShipSinkedArgs { ship = gameObject } );
        }
        else
        {
            _currentHealth--;
        }
    }
    
    public bool GetIsSinked()
    {
        return _currentHealth == 0;
    }
        
    public List<Vector2Int> GetGirdOffsetList(Vector2Int offset) {
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
        
    private void Awake()
    {
        _direction = Direction.Up;
        
        OnShipSinked = new UnityEvent<OnShipSinkedArgs>();
    }
    
    private void Start()
    {
        _currentHealth = _health;
    }
    
    private void LateUpdate()
    {
        if (_targetPosition != transform.position) 
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 15f);
        }
    }
}
