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
        private Tilemap _ghostTilemap;

        [SerializeField]
        private Piece _trackingPiece;
        
        [SerializeField]
        private TetrominoData[] _tetrominoes;
    
        [SerializeField]
        private Tile _ghostTile;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Board>().AsSingle().WithArguments(new object[]
            {
                _boardTilemap, _trackingPiece, _tetrominoes
            }).NonLazy();
            
            Container.Bind<Ghost>().AsSingle().WithArguments(
                _ghostTilemap, _ghostTile, _trackingPiece
            ).NonLazy();
        }
    }
}