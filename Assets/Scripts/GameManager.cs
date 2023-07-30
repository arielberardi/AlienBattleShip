using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _origin;
    [SerializeField] private GameObject[] _shipPrefabPlayer1Array;
    [SerializeField] private GameObject[] _shipPrefabPlayer2Array;
    
    [SerializeField] private Player _player1;
    [SerializeField] private Player _player2;
    
    private const int PLAYER_1 = 1;
    private const int PLAYER_2 = 2;
    
    private Player _currentPlayer;
    private int _currentPlayerId;
    
    private void Start()
    {
        _currentPlayerId = PLAYER_1;
        _currentPlayer = _player1;
        
        _player1.Setup(_width, _height, _cellSize, _origin);
        _player2.Setup(_width, _height, _cellSize, _origin);
        _player2.Hide();
    }
    
    private void Update()
    {
        ShipActions();
    }
    
    private void ShipActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentPlayer.Place();
        }
                
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentPlayer.Rotate();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            GrabShipForCurrentPlayer(0);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            GrabShipForCurrentPlayer(1);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
           GrabShipForCurrentPlayer(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentPlayer.Hide();
            
            _currentPlayer = _currentPlayerId == PLAYER_1 ?  _player2 : _player1;
            _currentPlayerId = _currentPlayerId == PLAYER_1 ? PLAYER_2 : PLAYER_1;
            
            _currentPlayer.Show();
        }
    }
    
    private void GrabShipForCurrentPlayer(int index)
    {
        if (_currentPlayerId == PLAYER_1)
        {
            _currentPlayer.Grab(_shipPrefabPlayer1Array[index]);
        }
        else
        {
            _currentPlayer.Grab(_shipPrefabPlayer2Array[index]);
        }
    }
}
