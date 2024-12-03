using System;
using BoardManagementModule;
using GameManagementModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Controllers
{
    public class UIController : IDisposable
    {
        public event Action ContinueButtonClickedEvent;
        public event Action QuitButtonClickedEvent;
        
        private readonly GameObject _menu;

        private readonly Button _playButton;
        private readonly Button _quitButton;
        
        private readonly TextMeshProUGUI _playText;
        private readonly TextMeshProUGUI _scoreText;
        
        public UIController(GameObject menu, Button playGameButton, Button quitButton, TextMeshProUGUI scoreText)
        {
            _menu = menu;
            _playButton = playGameButton;
            _quitButton = quitButton;
            _scoreText = scoreText;
            
            _playText = _playButton.GetComponentInChildren<TextMeshProUGUI>();
            
            ListenEvents();
        }

        private void MenuSetActive(bool isActive)
        {
            _menu.SetActive(isActive);
        }
        
        private void ListenEvents()
        {
            _playButton.onClick.AddListener(() =>
            {
                ContinueButtonClickedEvent?.Invoke();
            });
            
            _quitButton.onClick.AddListener(() =>
            {
                QuitButtonClickedEvent?.Invoke();
            });

            GameController.OnGameStateChanged += HandleGameStateChange;
            Board.ScoreChangeEvent += HandleScoreChange;
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Playing:
                    MenuSetActive(false);
                    break;
                
                case GameState.Paused:
                    _playText.text = "Continue";
                    MenuSetActive(true);
                    break;
                
                case GameState.Ready:
                case GameState.GameOver:
                    _playText.text = "Play";
                    MenuSetActive(true);
                    break;
            }
        }
        
        private void HandleScoreChange(int score)
        {
            _scoreText.text = "Score: " + score;
        }
        
        private void UnsubscribeFromEvents()
        {
            _playButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            GameController.OnGameStateChanged -= HandleGameStateChange;
            Board.ScoreChangeEvent -= HandleScoreChange;
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }
}