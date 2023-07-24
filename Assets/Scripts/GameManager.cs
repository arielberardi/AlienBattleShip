using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    
    [SerializeField] private MapGridVisual _mapGridVisual;
    
    private Grid2D<MapGridObject> _grid;
    
    // Start is called before the first frame update
    void Start()
    {
        _grid = new Grid2D<MapGridObject>(
            _width,
            _height,
            _cellSize,
            transform.position,
            (Grid2D<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y)
        );
        
        _mapGridVisual.Setup(_grid);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
