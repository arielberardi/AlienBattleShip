using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    [SerializeField] private MapGridVisual _shipPlaceGrid;  
    [SerializeField] private ShipAttackSystem _shipAttackSystem;
    [SerializeField] private MapGridVisual _shipAttackGrid;
    
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
        
        _shipPlaceGrid.Setup(_grid, MapGridVisual.MapType.Place);
        _shipSnapSystem.Setup(_grid);
        
        _shipAttackGrid.Setup(_grid, MapGridVisual.MapType.Attack);
        _shipAttackSystem.Setup(_grid);
    }
    
    public void Hide()
    {
        _shipPlaceGrid.Hide();
        _shipSnapSystem.Hide();
        _shipAttackGrid.Hide();
        _shipAttackSystem.Hide();
    }
    
    public void Show()
    {
        _shipPlaceGrid.Show();
        _shipSnapSystem.Show();
        _shipAttackGrid.Show();
        _shipAttackSystem.Show();
    }
    
    public void PrepareDeploy()
    {
        _shipPlaceGrid.Show();
        _shipSnapSystem.Show();
    }
    
    public void Deploy()
    {
        _shipSnapSystem.Deploy();
    }
    
    public void Grab(GameObject prefab)
    {
        _shipSnapSystem.Grab(prefab);
    }
    
    public void Rotate()
    {
        _shipSnapSystem.Rotate();
    }
    
    public void PrepareAttack()
    {
        _shipAttackGrid.Show();
        _shipAttackSystem.Show();
    }
    
    public bool Attack()
    {
        return _shipAttackSystem.Attack();
    }
    
    private void Update()
    {
        _shipPlaceGrid.Update();
        _shipAttackGrid.Update();
    }
}
