using UnityEngine;

namespace PVZ
{
    public static class GridUtilities
    {
        private static Vector3 _zeroWorldPosition = new Vector3(-4.5f, -2);
        private static Vector2Int _gridSize = new Vector2Int(10, 5);

        public static Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector3(_zeroWorldPosition.x + gridPosition.x,
                _zeroWorldPosition.y + gridPosition.y);
        }

        public static Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x - _zeroWorldPosition.x),
                Mathf.RoundToInt(worldPosition.y - _zeroWorldPosition.y));
        }

        public static bool PointIsInGrid(Vector2Int point)
        {
            return point.x >= 0 && point.x < _gridSize.x
                && point.y >= 0 && point.y < _gridSize.y;
        }
    }
}
