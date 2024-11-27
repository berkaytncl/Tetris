using BoardManagementModule;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class Ghost : MonoBehaviour
{
    private Board _board;
    
    private Tilemap _tilemap;
    
    private Tile _tile;
    
    private Piece _trackingPiece;
    
    public Vector2Int[] cells { get; private set; }
    public Vector2Int position { get; private set; }

    [Inject]
    public void ZenjectSetup(Board board, Tilemap tilemap, Tile tile, Piece piece)
    {
        _board = board;
        _tilemap = tilemap;
        _tile = tile;
        _trackingPiece = piece;
    }
    
    private void Awake()
    {
        cells = new Vector2Int[4];
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
        foreach (var t in cells)
        {
            Vector2Int tilePosition = t + position;
            _tilemap.SetTile((Vector3Int)tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = _trackingPiece.Cells[i];
        }
    }

    private void Drop()
    {
        Vector2Int position = _trackingPiece.Position;

        int current = position.y;
        int bottom = -_board.boardSize.y / 2 - 1;

        _board.Clear(_trackingPiece);

        for (int row = current; row >= bottom; row--)
        {
            position.y = row;

            if (_board.IsValidPosition(_trackingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }

        _board.Set(_trackingPiece);
    }

    private void Set()
    {
        foreach (var t in cells)
        {
            Vector2Int tilePosition = t + position;
            _tilemap.SetTile((Vector3Int)tilePosition, _tile);
        }
    }
}