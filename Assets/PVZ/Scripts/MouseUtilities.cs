using UnityEngine;
using UnityEngine.InputSystem;

namespace PVZ
{
    public static class MouseUtilities
    {
        private static Camera _camera = null;

        public static bool IsPressed => Mouse.current.press.isPressed;
        public static Vector2 ScreenPosition => Mouse.current.position.ReadValue();
        public static Vector3 WorldPosition
        {
            get
            {
                if (!_camera) _camera = Camera.main;
                return _camera.ScreenToWorldPoint(ScreenPosition);
            }
        }
        public static Vector2Int GridPosition => GridUtilities.WorldToGrid(WorldPosition);
    }
}
