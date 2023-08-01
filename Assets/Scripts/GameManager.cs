using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _origin;
    
    [SerializeField] private float _shipDeployTimeout = 20.0f;
    [SerializeField] private float _shipAttackTimeout = 20.0f;
    private float _shipDeployTimer;
    private float _shipAttackTimer;

    private const int PLAYER_1 = 1;
    private const int PLAYER_2 = 2;

    [SerializeField] private Player _player1;
    [SerializeField] private Player _player2;
    private Player _currentPlayer;
    private int _currentPlayerId;    
    
    private void Start()
    {
        _lastGameState = GameState.Idle;
        _currentGameState = GameState.ShipDeployPlayer1;
        
        _player1.Setup(_width, _height, _cellSize, _origin, ShipSnapSystem.ShipTeam.Blue);
        _player2.Setup(_width, _height, _cellSize, _origin, ShipSnapSystem.ShipTeam.Red);
        _player1.Hide();
        _player2.Hide();
    }
    
    private void Update()
    {
        switch (_currentGameState)
        {
            case GameState.ShipDeployPlayer1: ShipDeploy(PLAYER_1); break;
            case GameState.ShipDeployPlayer2: ShipDeploy(PLAYER_2); break;
            case GameState.ShipAttackPlayer1: ShipAttack(PLAYER_1, PLAYER_2); break;
            case GameState.ShipAttackPlayer2: ShipAttack(PLAYER_2, PLAYER_1); break;
        }
    }
    
    private void ShipDeploy(int playerId)
    {
        if (playerId == PLAYER_1)
        {
            if (_currentPlayerId != PLAYER_1)
            {
                _player1.PrepareDeploy();
                _player2.Hide();
                
                _currentPlayerId = playerId;
                _currentPlayer = _player1;
                
                _shipDeployTimer = _shipDeployTimeout;
                Debug.Log("Ship Deploy for Player 1");
            }
        }
        else
        {
            if (_currentPlayerId != PLAYER_2)
            {
                _player1.Hide();
                _player2.PrepareDeploy();
                
                _currentPlayerId = playerId;
                _currentPlayer = _player2;
                
                _shipDeployTimer = _shipDeployTimeout;
                Debug.Log("Ship Deploy for Player 2");
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _currentPlayer.Deploy();
        }
                
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentPlayer.Rotate();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _currentPlayer.Grab(ShipSnapSystem.ShipType.Atlas);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentPlayer.Grab(ShipSnapSystem.ShipType.Mother);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
           _currentPlayer.Grab(ShipSnapSystem.ShipType.Spy);
        }
        
        if (_shipDeployTimer > 0)
        {
            _shipDeployTimer -= Time.deltaTime;
        }
        else
        {
            _lastGameState = _currentGameState;
            _currentGameState++;
        }
    }
    
    private void ShipAttack(int playerId, int enemyId)
    {
        if (playerId == PLAYER_1) 
        {
            if (_currentPlayerId != PLAYER_1)
            {
                _player2.Hide();
                _player1.PrepareAttack();
                
                _currentPlayerId = playerId;
                _currentPlayer = _player1;
                
                _shipAttackTimer = _shipAttackTimeout;
                Debug.Log("Ship Attack for Player 1");
            }
        }
        else
        {
            if (_currentPlayerId != PLAYER_2)
            {
                _player1.Hide();
                _player2.PrepareAttack();
                
                _currentPlayerId = playerId;
                _currentPlayer = _player2;
                
                _shipAttackTimer = _shipAttackTimeout;
                Debug.Log("Ship Attack for Player 2");
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int attackPosition = _currentPlayer.Attack();
            if (attackPosition.x != -1)
            {
                if (_currentPlayerId == PLAYER_1)
                {
                    _player2.Hit(attackPosition);
                }
                else if (_currentPlayerId == PLAYER_2)
                {
                    _player1.Hit(attackPosition);
                }
                
                _shipAttackTimer = 0;
            }
        }
        
        if (_shipAttackTimer > 0)
        {
            _shipAttackTimer -= Time.deltaTime;
        }
        else
        {
            _lastGameState = _currentGameState;
            if (_currentGameState == GameState.ShipAttackPlayer1)
            {
                _currentGameState = GameState.ShipAttackPlayer2;
            }
            else
            {
                _currentGameState = GameState.ShipAttackPlayer1;
            }
        }
    }
}
