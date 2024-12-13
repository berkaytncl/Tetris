using System;
using BoardManagementModule;
using UIModule.Controllers;
using UnityEngine;
using Utility.Managers;
using Zenject;

namespace GameManagementModule
{
    public class GameController : MonoBehaviour
    {
        public static event Action<GameState> OnGameStateChanged;

        [Inject]
        private readonly BoardController _boardController;

        [Inject]
        private readonly UIController _uiController;
        
        private ScoreManager _scoreManager;
        
        private void Awake()
        {
            ListenEvents();
            
            _scoreManager = new ScoreManager();
        }
        
        private void Start()
        {
            OnGameStateChanged?.Invoke(_scoreManager.GameData.GameState);
            Time.timeScale = 0f;
        }
        
        private void ListenEvents()
        {
            _uiController.ContinueButtonClickedEvent += ContinueGame;
            _uiController.QuitButtonClickedEvent += QuitGame;
            _uiController.MenuButtonClickedEvent += ReturnMenu;
            
            _boardController.UpdateScoreEvent += HandleUpdateScore;
            _boardController.GameOverEvent += HandleGameOver;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_scoreManager.GameData.GameState == GameState.Paused)
                {
                    ContinueGame();
                }
                else if (_scoreManager.GameData.GameState == GameState.Playing)
                {
                    PauseGame();
                }
            }
        }
        
        private void HandleUpdateScore(int score)
        {
            _scoreManager.AddToCurrentScore(score);
        }

        private void ContinueGame()
        {
            _scoreManager.GameData.GameState = GameState.Playing;
            OnGameStateChanged?.Invoke(_scoreManager.GameData.GameState);
            Time.timeScale = 1f;
        }

        private void PauseGame()
        {
            _scoreManager.GameData.GameState = GameState.Paused;
            OnGameStateChanged?.Invoke(_scoreManager.GameData.GameState);
            Time.timeScale = 0f;
        }

        private void ReturnMenu()
        {
            _scoreManager.NewGame();
            OnGameStateChanged?.Invoke(_scoreManager.GameData.GameState);
        }
        
        private void HandleGameOver()
        {
            _scoreManager.GameData.GameState = GameState.GameOver;
            OnGameStateChanged?.Invoke(_scoreManager.GameData.GameState);
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
            _uiController.MenuButtonClickedEvent -= ReturnMenu;
            
            _boardController.UpdateScoreEvent -= HandleUpdateScore;
            _boardController.GameOverEvent -= HandleGameOver;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
    }
}