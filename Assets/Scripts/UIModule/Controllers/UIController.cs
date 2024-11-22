using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Controllers
{
    public class UIController
    {
        private readonly GameObject _pauseMenu;

        private readonly Button _continueButton;

        private readonly Button _quitButton;

        public UIController(GameObject pauseMenu, Button continueButton, Button quitButton)
        {
            _pauseMenu = pauseMenu;
            _continueButton = continueButton;
            _quitButton = quitButton;
        }
    }
}