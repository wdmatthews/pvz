using UnityEngine;

namespace PVZ.Plants
{
    [CreateAssetMenu(fileName = "New Plant", menuName = "PVZ/Plants/Plant")]
    public class PlantSO : ScriptableObject
    {
        public Plant Prefab = null;
        public Sprite Icon = null;
        public int Cost = 20;
        public float PlaceCooldown = 2;
    }
}
