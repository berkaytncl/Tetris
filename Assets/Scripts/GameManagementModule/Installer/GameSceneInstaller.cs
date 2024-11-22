using UIModule.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameManagementModule.Installer
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _pauseMenu;

        [SerializeField]
        private Button _continueButton;
        [SerializeField]
        private Button _quitButton;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle().WithArguments(new object[]
            {
                _pauseMenu, _continueButton, _quitButton
            }).NonLazy();
        }
    }
}