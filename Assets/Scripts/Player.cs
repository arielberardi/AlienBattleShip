using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    [SerializeField] private MapGridVisual _shipSnapGrid;  
    [SerializeField] private ShipAttackSystem _shipAttackSystem;
    [SerializeField] private MapGridVisual _shipAttackGrid;
    
    private Grid2D<MapGridObject> _grid;
    
    // Start is called before the first frame update
    public void Setup(int width, int height, float cellSize, Vector3 origin, ShipSnapSystem.ShipTeam team)
    {
        _grid = new Grid2D<MapGridObject>(
            width,
            height,
            cellSize,
            origin, 
            (Grid2D<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y)
        );
        
        _shipSnapGrid.Setup(_grid, MapGridVisual.MapType.Place);
        _shipSnapSystem.Setup(_grid, team);
        
        _shipAttackGrid.Setup(_grid, MapGridVisual.MapType.Attack);
        _shipAttackSystem.Setup(_grid);
    }
    
    public void Hide()
    {
        _shipSnapGrid.Hide();
        _shipSnapSystem.Hide();
        _shipAttackGrid.Hide();
    }
    
    public void Show()
    {
        _shipSnapGrid.Show();
        _shipSnapSystem.Show();
        _shipAttackGrid.Show();
    }
    
    public void PrepareDeploy()
    {
        _shipSnapGrid.Show();
        _shipSnapSystem.Show();
    }
    
    public void Deploy()
    {
        _shipSnapSystem.Deploy();
    }
    
    public void Grab(ShipSnapSystem.ShipType type)
    {
        _shipSnapSystem.Grab(type);
    }
    
    public void Rotate()
    {
        _shipSnapSystem.Rotate();
    }
    
    public void PrepareAttack()
    {
        _shipAttackGrid.Show();
    }
    
    public bool Attack()
    {
        return _shipAttackSystem.Attack();
    }
    
    private void Update()
    {
        _shipSnapGrid.Update();
        _shipAttackGrid.Update();
    }
}
