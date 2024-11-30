using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = UnityEngine.Random;

namespace BoardManagementModule
{
    public class Board
    {
        public readonly Vector2Int BOARD_SIZE = new Vector2Int(10, 20);

        private readonly Tilemap _tilemap;

        private readonly Piece _activePiece;
    
        private readonly TetrominoData[] _tetrominoes;
        
        private readonly Vector2Int _spawnPosition = new Vector2Int(-1, 8);
    
        private RectInt Bounds
        {
            get
            {
                Vector2Int position = new Vector2Int(-BOARD_SIZE.x / 2, -BOARD_SIZE.y / 2);
                return new RectInt(position, BOARD_SIZE);
            }
        }
        
        public Board(Tilemap tilemap, Piece activePiece, TetrominoData[] tetrominoes)
        {
            _tilemap = tilemap;
            _activePiece = activePiece;
            _tetrominoes = tetrominoes;
        
            SpawnPiece();
        }

        public Piece GetPiece()
        {
            return _activePiece;
        }
    
        public void SpawnPiece()
        {
            int random = Random.Range(0, _tetrominoes.Length);
            TetrominoData data = _tetrominoes[random];
        
            _activePiece.Initialize(_spawnPosition, data);

            if (IsValidPosition(_activePiece, _spawnPosition))
            {
                Set(_activePiece);
            }
            else
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            _tilemap.ClearAllTiles();
        }

        public void Set(Piece piece)
        {
            foreach (var t in piece.Cells)
            {
                Vector2Int tilePosition = t + piece.Position;
                _tilemap.SetTile(tilePosition, piece.GetTile());
            }
        }

        public void Clear(Piece piece)
        {
            foreach (var t in piece.Cells)
            {
                Vector2Int tilePosition = t + piece.Position;
                _tilemap.SetTile(tilePosition, null);
            }
        }

        public bool IsValidPosition(Piece piece, Vector2Int position)
        {
            RectInt bounds = Bounds;

            foreach (var t in piece.Cells)
            {
                Vector2Int tilePosition = t + position;

                if (!bounds.Contains(tilePosition))
                {
                    return false;
                }

                if (_tilemap.HasTile(tilePosition))
                {
                    return false;
                }
            }

            return true;
        }

        public void ClearLines()
        {
            for (int row = -10; row < 10;)
            {
                if (IsLineFull(row))
                {
                    LineClear(row);
                }
                else
                {
                    row++;
                }
            }
        }

        private bool IsLineFull(int row)
        {
            for (int col = -5; col < 5; col++)
            {
                Vector2Int position = new Vector2Int(col, row);
                if (!_tilemap.HasTile(position))
                {
                    return false;
                }
            }

            return true;
        }

        private void LineClear(int row)
        {
            for (int col = -5; col < 5; col++)
            {
                Vector2Int position = new Vector2Int(col, row);
                _tilemap.SetTile(position, null);
            }

            for (int rowIndex = row; rowIndex < 10; rowIndex++)
            {
                for (int col = -5; col < 5; col++)
                {
                    Vector2Int position = new Vector2Int(col, rowIndex + 1);
                    TileBase above = _tilemap.GetTile(position);

                    position = new Vector2Int(col, rowIndex);
                    _tilemap.SetTile(position, above);
                }
            }
        }
    }
}