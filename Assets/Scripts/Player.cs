using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    [SerializeField] private MapGridVisual _mapGridVisual;  
    
    private Grid2D<MapGridObject> _grid;
    
    // Start is called before the first frame update
    public void Setup(int width, int height, float cellSize, Vector3 origin)
    {
        _grid = new Grid2D<MapGridObject>(
            width,
            height,
            cellSize,
            origin,
            (Grid2D<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y)
        );
        
        _mapGridVisual.Setup(_grid);
        _shipSnapSystem.Setup(_grid);
    }
    
    public void Place()
    {
        _shipSnapSystem.Place();
    }
    
    public void Grab(GameObject prefab)
    {
        _shipSnapSystem.Grab(prefab);
    }
    
    public void Rotate()
    {
        _shipSnapSystem.Rotate();
    }
    
    public void Hide()
    {
        _mapGridVisual.Hide();
        _shipSnapSystem.Hide();
    }
    
    public void Show()
    {
        _mapGridVisual.Show();
        _shipSnapSystem.Show();
    }
    
    private void Update()
    {
        _mapGridVisual.Update();
    }
}
