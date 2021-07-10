using UnityEngine;

namespace PVZ.Plants
{
    [CreateAssetMenu(fileName = "New Plant", menuName = "PVZ/Plants/Plant")]
    public class PlantSO : ScriptableObject
    {
        public Plant Prefab = null;
        public int SunCost = 20;
    }
}
