using UnityEngine;

namespace PVZ.Combat
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "PVZ/Combat/Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        public Projectile Prefab = null;
        public bool IsFromPlant = true;
        public float MoveSpeed = 2;
        public int Damage = 5;
        public float AttackRange = 5;
        public float DamageRange = 0.25f;
    }
}
