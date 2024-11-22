using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    public static class TilemapHelper
    {
        public static void SetTile(this Tilemap tilemap, Vector2Int position, TileBase tileBase)
        {
            tilemap.SetTile(new Vector3Int(position.x, position.y, 0), tileBase);
        }

        public static TileBase GetTile(this Tilemap tilemap, Vector2Int position)
        {
            return tilemap.GetTile(new Vector3Int(position.x, position.y, 0));
        }

        public static bool HasTile(this Tilemap tilemap, Vector2Int position)
        {
            return tilemap.HasTile(new Vector3Int(position.x, position.y, 0));
        }
    }
}