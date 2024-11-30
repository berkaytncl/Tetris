using BoardManagementModule;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class Piece : MonoBehaviour
{
    [Inject]
    private Board _board;
    
    private Vector2Int[] _cells;
    
    private Vector2Int[,] _wallKicks;
    
    public Vector2Int[] Cells => _cells;
    
    public Vector2Int Position { get; private set; }

    private TetrominoData _tetrominoData;

    private int _rotationIndex;

    private const float STEP_DELAY = 1f;
    private const float MOVE_DELAY = 0.1f;
    private const float LOCK_DELAY = 0.5f;

    private float _stepTime;
    private float _moveTime;
    private float _lockTime;

    public void Initialize(Vector2Int position, TetrominoData tetrominoData)
    {
        Position = position;

        _tetrominoData = tetrominoData;

        _rotationIndex = 0;
        _stepTime = Time.time + STEP_DELAY;
        _moveTime = Time.time + MOVE_DELAY;
        _lockTime = 0f;

        TetrominoType tetrominoType = tetrominoData.tetrominoType;
        
        int cellLength = BoardData.Cells[tetrominoType].Length;
        
        _cells = new Vector2Int[cellLength];
        _wallKicks = BoardData.WallKicks[tetrominoType];
        
        for (int index = 0; index < cellLength; index++)
        {
            _cells[index] = BoardData.Cells[tetrominoType][index];
        }
    }

    private void Update()
    {
        _board.Clear(this);

        _lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time > _moveTime)
        {
            HandleMoveInputs();
        }

        if (Time.time > _stepTime)
        {
            Step();
        }

        _board.Set(this);
    }

    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (Move(Vector2Int.down))
            {
                _stepTime = Time.time + STEP_DELAY;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector2Int.left);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
    }

    private void Step()
    {
        _stepTime = Time.time + STEP_DELAY;

        Move(Vector2Int.down);

        if (_lockTime >= LOCK_DELAY)
        {
            Lock();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down)) { }

        Lock();
    }

    private void Lock()
    {
        _board.Set(this);
        _board.ClearLines();
        _board.SpawnPiece();
    }

    private bool Move(Vector2Int translation)
    {
        Vector2Int newPosition = Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = _board.IsValidPosition(this, newPosition);

        if (valid)
        {
            Position = newPosition;
            _moveTime = Time.time + MOVE_DELAY;
            _lockTime = 0f;
        }

        return valid;
    }

    public TileBase GetTile()
    {
        return _tetrominoData.tile;
    }

    private void Rotate(int direction)
    {
        int originalRotation = _rotationIndex;

        _rotationIndex = Wrap(_rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(_rotationIndex, direction))
        {
            _rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = BoardData.RotationMatrix;

        for (int i = 0; i < _cells.Length; i++)
        {
            Vector2 cell = _cells[i];

            int x, y;

            switch (_tetrominoData.tetrominoType)
            {
                case TetrominoType.I:
                case TetrominoType.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            _cells[i] = new Vector2Int(x, y);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < _wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = _wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, _wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
            
        return min + (input - min) % (max - min);
    }
}