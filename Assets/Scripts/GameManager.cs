using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    
    public enum GameState 
    {
        Idle,
        ShipDeployPlayer1,
        ShipDeployPlayer2,
        ShipAttackPlayer1,
        ShipAttackPlayer2
    };
    private GameState _currentGameState;
    private GameState _lastGameState;
    
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _gridMap;
    
    private void Awake()
    {
        Singleton = this;
    }
    
    private void Start()
    {
        _lastGameState = GameState.Idle;
        _currentGameState = GameState.ShipDeployPlayer1;
        
        // Player 1
        Transform player1 = Instantiate(_player, Vector3.zero, Quaternion.identity).GetComponent<Transform>();
        Instantiate(_gridMap, new Vector3(-50, -50, 0), Quaternion.identity, player1);
    }
    
    private void Update()
    {
        switch (_currentGameState)
        {
            case GameState.ShipDeployPlayer1: break;
            case GameState.ShipDeployPlayer2: break;
            case GameState.ShipAttackPlayer1: break;
            case GameState.ShipAttackPlayer2: break;
        }
        
        _lastGameState = _currentGameState;
    }
}
