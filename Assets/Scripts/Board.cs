using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Piece _activePiece;

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector2Int spawnPosition = new Vector2Int(-1, 8);
    
    private RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        _activePiece = GetComponentInChildren<Piece>();
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        
        _activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(_activePiece, spawnPosition))
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
        tilemap.ClearAllTiles();
    }

    public void Set(Piece piece)
    {
        foreach (var t in piece.Cells)
        {
            Vector2Int tilePosition = t + piece.Position;
            tilemap.SetTile(tilePosition, piece.GetTile());
        }
    }

    public void Clear(Piece piece)
    {
        foreach (var t in piece.Cells)
        {
            Vector2Int tilePosition = t + piece.Position;
            tilemap.SetTile(tilePosition, null);
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

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
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
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector2Int position = new Vector2Int(col, row);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector2Int position = new Vector2Int(col, row);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector2Int position = new Vector2Int(col, row + 1);
                TileBase above = tilemap.GetTile(position);

                position = new Vector2Int(col, row);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}