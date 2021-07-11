using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    [CreateAssetMenu(fileName = "New Plant", menuName = "PVZ/Plants/Plant")]
    public class PlantSO : DamageableSO
    {
        public Plant Prefab = null;
        public Sprite Icon = null;
        public int Cost = 20;
        public float PlaceCooldown = 2;
    }
}
