using BoardManagementModule;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Utility;

namespace BoardManagementModule
{
    public class Ghost : MonoBehaviour
    {
        [Inject] 
        private BoardController _boardController;
        
        [SerializeField] 
        private Tilemap _tilemap;

        [SerializeField] 
        private Tile _ghostTile;

        private Piece _piece;

        private Vector2Int[] _cells;

        private Vector2Int _position;

        private void Awake()
        {
            _cells = new Vector2Int[4];
            _piece = _boardController.GetPiece();
        }

        private void LateUpdate()
        {
            Clear();
            Copy();
            Drop();
            Set();
        }

        private void Clear()
        {
            foreach (var t in _cells)
            {
                Vector2Int tilePosition = t + _position;
                _tilemap.SetTile(tilePosition, null);
            }
        }

        private void Copy()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = _piece.Cells[i];
            }
        }

        private void Drop()
        {
            Vector2Int position = _piece.Position;

            int current = position.y;
            int bottom = -_boardController.BOARD_SIZE.y / 2 - 1;

            _boardController.Clear(_piece);

            for (int row = current; row >= bottom; row--)
            {
                position.y = row;

                if (_boardController.IsValidPosition(_piece, position))
                {
                    _position = position;
                }
                else
                {
                    break;
                }
            }

            _boardController.Set(_piece);
        }

        private void Set()
        {
            foreach (var t in _cells)
            {
                Vector2Int tilePosition = t + _position;
                _tilemap.SetTile(tilePosition, _ghostTile);
            }
        }
    }
}