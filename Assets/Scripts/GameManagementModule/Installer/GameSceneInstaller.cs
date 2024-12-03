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
        private Button _playButton;
        [SerializeField]
        private Button _quitButton;
        
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle().WithArguments(new object[]
            {
                _menu, _playButton, _quitButton, _scoreText
            }).NonLazy();
        }
    }
}