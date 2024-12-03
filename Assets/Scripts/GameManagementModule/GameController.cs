using System;
using BoardManagementModule;
using UIModule.Controllers;
using UnityEngine;
using Zenject;

namespace GameManagementModule
{
    public class GameController : MonoBehaviour
    {
        public static event Action<GameState> OnGameStateChanged;

        private GameState _gameState = GameState.Ready;

        [Inject]
        private readonly UIController _uiController;
        
        private void Awake()
        {
            ListenEvents();
        }
        
        private void Start()
        {
            OnGameStateChanged?.Invoke(_gameState);
            Time.timeScale = 0f;
        }
        
        private void ListenEvents()
        {
            _uiController.ContinueButtonClickedEvent += ContinueGame;
            _uiController.QuitButtonClickedEvent += QuitGame;
            Board.GameOverEvent += HandleGameOver;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_gameState == GameState.Paused)
                {
                    ContinueGame();
                }
                else if (_gameState == GameState.Playing)
                {
                    PauseGame();
                }
            }
        }

        private void ContinueGame()
        {
            _gameState = GameState.Playing;
            OnGameStateChanged?.Invoke(_gameState);
            Time.timeScale = 1f;
        }

        private void PauseGame()
        {
            _gameState = GameState.Paused;
            OnGameStateChanged?.Invoke(_gameState);
            Time.timeScale = 0f;
        }

        private void HandleGameOver()
        {
            _gameState = GameState.GameOver;
            OnGameStateChanged?.Invoke(_gameState);
            Time.timeScale = 0f;
        }
        
        private void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        private void UnsubscribeFromEvents()
        {
            _uiController.ContinueButtonClickedEvent -= ContinueGame;
            _uiController.QuitButtonClickedEvent -= QuitGame;
            Board.GameOverEvent -= HandleGameOver;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
    }
}