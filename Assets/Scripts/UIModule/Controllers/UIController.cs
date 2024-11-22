using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Controllers
{
    public class UIController : IDisposable
    {
        public event Action ContinueButtonClickedEvent;
        public event Action QuitButtonClickedEvent;
        
        private readonly GameObject _pauseMenu;

        private readonly Button _continueButton;
        private readonly Button _quitButton;
        
        public UIController(GameObject pauseMenu, Button continueButton, Button quitButton)
        {
            _pauseMenu = pauseMenu;
            _continueButton = continueButton;
            _quitButton = quitButton;
            
            ListenEvents();
        }

        public void TogglePauseMenu(bool toggle)
        {
            _pauseMenu.SetActive(toggle);
        }
        
        private void ListenEvents()
        {
            _continueButton.onClick.AddListener(() =>
            {
                ContinueButtonClickedEvent?.Invoke();
            });
            
            _quitButton.onClick.AddListener(() =>
            {
                QuitButtonClickedEvent?.Invoke();
            });
        }

        private void UnsubscribeFromEvents()
        {
            _continueButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }
}