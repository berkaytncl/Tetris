using UnityEngine;
using Zenject;
using UnityEngine.Tilemaps;

namespace BoardManagementModule.Installer
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField]
        private Tilemap _boardTilemap;

        [SerializeField]
        private Piece _trackingPiece;
        
        [SerializeField]
        private TetrominoData[] _tetrominoes;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Board>().AsSingle().WithArguments(new object[]
            {
                _boardTilemap, _trackingPiece, _tetrominoes
            }).NonLazy();
        }
    }
}