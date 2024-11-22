using System;
using UIModule.Controllers;
using UnityEngine;
using Zenject;

namespace GameManagementModule
{
    public class GameController : MonoBehaviour
    {
        public static event Action<GameState> OnGameStateChanged;

        private GameState _gameState;

        [Inject]
        private readonly UIController _uiController;
        
        private void Awake()
        {
            ListenEvents();
        }
        
        private void Start()
        {
            _gameState = GameState.Ready;
            OnGameStateChanged?.Invoke(_gameState);
        }
        
        private void ListenEvents()
        {
            _uiController.ContinueButtonClickedEvent += ContinueGame;
            _uiController.QuitButtonClickedEvent += QuitGame;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_gameState == GameState.Paused)
                {
                    ContinueGame();
                }

                if (_gameState == GameState.Playing)
                {
                    PauseGame();
                }
            }
        }

        private void ContinueGame()
        {
            _uiController.TogglePauseMenu(false);
            Time.timeScale = 1f;
            _gameState = GameState.Playing;
            OnGameStateChanged?.Invoke(_gameState);
        }

        private void PauseGame()
        {
            _uiController.TogglePauseMenu(true);
            Time.timeScale = 0f;
            _gameState = GameState.Paused;
            OnGameStateChanged?.Invoke(_gameState);
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
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
    }
}