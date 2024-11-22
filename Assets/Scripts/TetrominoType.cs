using UnityEngine.Tilemaps;

public enum TetrominoType
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tile tile;
    public TetrominoType tetrominoType;
}