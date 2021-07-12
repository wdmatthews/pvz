using UnityEngine;

namespace PVZ
{
    public static class GridUtilities
    {
        public static readonly Vector3 ZeroWorldPosition = new Vector3(-4.5f, -2);
        public static readonly Vector2Int GridSize = new Vector2Int(10, 5);
        public const float EndScreenWorldPosition = 6.5f;
        public const float GameOverWorldPosition = -5.5f;

        public static Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector3(ZeroWorldPosition.x + gridPosition.x,
                ZeroWorldPosition.y + gridPosition.y);
        }

        public static Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x - ZeroWorldPosition.x),
                Mathf.RoundToInt(worldPosition.y - ZeroWorldPosition.y));
        }

        public static bool PointIsInGrid(Vector2Int point)
        {
            return point.x >= 0 && point.x < GridSize.x
                && point.y >= 0 && point.y < GridSize.y;
        }
    }
}
