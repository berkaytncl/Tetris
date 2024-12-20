using System;
using System.Collections.Generic;
using System.Linq;
using GameManagementModule;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Managers;

namespace UIModule.Controllers
{
    public class UIController : IDisposable
    {
        public event Action ContinueButtonClickedEvent;
        public event Action QuitButtonClickedEvent;
        public event Action MenuButtonClickedEvent;
        
        private readonly GameObject _menu;
        private readonly GameObject _gameOverMenu;

        private readonly Button _playButton;
        private readonly Button _quitButton;
        private readonly Button _menuButton;
        
        private readonly TextMeshProUGUI _playText;
        private readonly TextMeshProUGUI _highestScoreText;
        private readonly TextMeshProUGUI _scoreText;
        private readonly TextMeshProUGUI _menuHighestScoreText;
        private readonly TextMeshProUGUI _menuScoreText;
        
        public UIController(
            GameObject menu, GameObject gameOverMenu, Button playButton, Button quitButton, Button menuButton,
            TextMeshProUGUI highestScoreText, TextMeshProUGUI scoreText, TextMeshProUGUI menuHighestScoreText,
            TextMeshProUGUI menuScoreText)
        {
            _menu = menu;
            _gameOverMenu = gameOverMenu;
            _playButton = playButton;
            _quitButton = quitButton;
            _menuButton = menuButton;
            _highestScoreText = highestScoreText;
            _scoreText = scoreText;
            _menuHighestScoreText = menuHighestScoreText;
            _menuScoreText = menuScoreText;
            
            _playText = _playButton.GetComponentInChildren<TextMeshProUGUI>();
            
            ListenEvents();
        }

        private void MenuSetActive(bool isActive)
        {
            _menu.SetActive(isActive);
        }
        
        private void GameOverMenuSetActive(bool isActive)
        {
            _gameOverMenu.SetActive(isActive);
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
            
            _menuButton.onClick.AddListener(() =>
            {
                MenuButtonClickedEvent?.Invoke();
            });

            GameController.OnGameStateChanged += HandleGameStateChange;
            ScoreManager.ScoreChangeEvent += HandleScoreChange;
            ScoreManager.TopScoresChangeEvent += HandleTopScoresChange;
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Ready:
                    _playText.text = "Play";
                    MenuSetActive(true);
                    GameOverMenuSetActive(false);
                    break;
                
                case GameState.Playing:
                    MenuSetActive(false);
                    GameOverMenuSetActive(false);
                    break;
                
                case GameState.Paused:
                    _playText.text = "Continue";
                    MenuSetActive(true);
                    GameOverMenuSetActive(false);
                    break;
                
                case GameState.GameOver:
                    MenuSetActive(false);
                    GameOverMenuSetActive(true);
                    break;
            }
        }
        
        private void HandleScoreChange(int score)
        {
            _scoreText.text = "Score: " + score;
            _menuScoreText.text = "Score: " + score;
        }

        private void HandleTopScoresChange(List<TopScoresData> topScores)
        {
            if (topScores == null || topScores.Count == 0) return;
            
            int highestScore = topScores.Max(scoreData => scoreData.Score);
            
            _highestScoreText.text = "High Score: " + highestScore;
            _menuHighestScoreText.text = "High Score: " + highestScore;
        }
        
        private void UnsubscribeFromEvents()
        {
            _playButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            _menuButton.onClick.RemoveAllListeners();
            GameController.OnGameStateChanged -= HandleGameStateChange;
            ScoreManager.ScoreChangeEvent -= HandleScoreChange;
            ScoreManager.TopScoresChangeEvent -= HandleTopScoresChange;
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }
}