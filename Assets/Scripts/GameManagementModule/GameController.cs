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
        public static event Action<int> ScoreChangeEvent;

        [Inject]
        private readonly UIController _uiController;
        
        private HighScoreManager _highScoreManager;
        
        private GameState _gameState = GameState.Ready;
        
        private int _currentScore;
        
        private void Awake()
        {
            ListenEvents();
            
            _highScoreManager = new HighScoreManager();
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
            _uiController.MenuButtonClickedEvent += ReturnMenu;
            Board.UpdateScoreEvent += HandleUpdateScore;
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
        
        private void HandleUpdateScore(int score)
        {
            _currentScore += score;
            ScoreChangeEvent?.Invoke(_currentScore);
        }

        private void ResetScore()
        {
            _currentScore = 0;
            ScoreChangeEvent?.Invoke(_currentScore);
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

        private void ReturnMenu()
        {
            _gameState = GameState.Ready;
            OnGameStateChanged?.Invoke(_gameState);
            ResetScore();
        }
        
        private void HandleGameOver()
        {
            _gameState = GameState.GameOver;
            OnGameStateChanged?.Invoke(_gameState);
            Time.timeScale = 0f;
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }

        private void UnsubscribeFromEvents()
        {
            _uiController.ContinueButtonClickedEvent -= ContinueGame;
            _uiController.QuitButtonClickedEvent -= QuitGame;
            Board.UpdateScoreEvent -= HandleUpdateScore;
            Board.GameOverEvent -= HandleGameOver;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
    }
}