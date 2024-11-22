using System;
using UnityEngine;

namespace GameManagementModule
{
    public class GameController : MonoBehaviour
    {
        public static event Action<GameState> OnGameStateChanged;

        private GameState _gameState;

        private void Start()
        {
            _gameState = GameState.Ready;
            OnGameStateChanged?.Invoke(_gameState);
        }
        
        public void EndGame()
        {
            Application.Quit();
        }
    }
}