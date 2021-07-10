using UnityEngine;

namespace PVZ
{
    public static class GridUtilities
    {
        private static Vector3 _zeroWorldPosition = new Vector3(-4.5f, -2);

        public static Vector3 CellToWorld(Vector2Int cellPosition)
        {
            return new Vector3(_zeroWorldPosition.x + cellPosition.x,
                _zeroWorldPosition.y + cellPosition.y);
        }
    }
}
