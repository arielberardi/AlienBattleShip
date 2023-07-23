using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    [SerializeField] private float _cellSize = 10f;
    
    
    private Grid _grid;
    
    void Start()
    {
        _grid = new Grid(_width, _height, _cellSize, new Vector3(20, 0, 0));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _grid.SetValue(worldPosition, 1);
        }
    }
}
