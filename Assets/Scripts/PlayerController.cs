using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ShipSnapSystem _shipSnapSystem;
    [SerializeField] private GridMap _shipSnapGrid;  
    [SerializeField] private ShipAttackSystem _shipAttackSystem;
    [SerializeField] private GridMap _shipAttackGrid;
    
    private Grid2D<GridObject> _grid;
    
    // Start is called before the first frame update
    public void Setup(int width, int height, float cellSize, Vector3 origin, ShipSnapSystem.ShipTeam team)
    {
        _grid = new Grid2D<GridObject>(
            width,
            height,
            cellSize,
            origin, 
            (Grid2D<GridObject> g, int x, int y) => new GridObject(g, x, y)
        );
        
        // _shipSnapGrid.Setup(_grid);
        // _shipSnapSystem.Setup(_grid, team);
        
        // _shipAttackGrid.Setup(_grid);
        // _shipAttackSystem.Setup(_grid);
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
    
    public Vector2Int Attack()
    {
        return _shipAttackSystem.Attack();
    }
    
    public bool Hit(Vector2Int hitPosition)
    {
        return _shipAttackSystem.Hit(hitPosition);
    }
    
    private void Update()
    {
        // _shipSnapGrid.Update();
        // _shipAttackGrid.Update();
    }
}
