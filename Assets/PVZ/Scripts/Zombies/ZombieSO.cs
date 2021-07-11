using UnityEngine;
using PVZ.Combat;

namespace PVZ.Zombies
{
    [CreateAssetMenu(fileName = "New Zombie", menuName = "PVZ/Zombies/Zombie")]
    public class ZombieSO : DamageableSO
    {
        public Zombie Prefab = null;
        public float MoveSpeed = 1;
        public float Range = 0.5f;
        public int Damage = 5;
        public float AttackCooldown = 1;
    }
}
