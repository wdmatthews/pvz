using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    [CreateAssetMenu(fileName = "New Attacking Plant", menuName = "PVZ/Plants/Attacking Plant")]
    public class AttackingPlantSO : PlantSO
    {
        public float AttackCooldown = 1;
        public ProjectileSO Projectile = null;
    }
}
