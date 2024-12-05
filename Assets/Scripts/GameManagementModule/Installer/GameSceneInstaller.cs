using TMPro;
using UIModule.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameManagementModule.Installer
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _menu;        
        [SerializeField]
        private GameObject _gameOverMenu;

        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private Button _quitButton;
        [SerializeField]
        private Button _menuButton;
        
        [SerializeField]
        private TextMeshProUGUI _highestScoreText;        
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private TextMeshProUGUI _menuHighestScoreText;        
        [SerializeField]
        private TextMeshProUGUI _menuScoreText;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle().WithArguments(new object[]
            {
                _menu, _gameOverMenu, _playButton, _quitButton, _menuButton, _highestScoreText, _scoreText,
                _menuHighestScoreText, _menuScoreText
            }).NonLazy();
        }
    }
}