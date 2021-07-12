using System.Collections.Generic;
using UnityEngine;

namespace PVZ.Combat
{
    [AddComponentMenu("PVZ/Combat/Lawn Mower")]
    [DisallowMultipleComponent]
    public class LawnMower : MonoBehaviour
    {
        private const float _moveSpeed = 3;
        private const float _triggerRange = 0.5f;
        private const float _damageRange = 0.25f;

        public Vector2Int Position { get; protected set; }
        private bool _isMowing = false;

        public virtual void Spawn(Vector2Int spawnPosition)
        {
            Position = spawnPosition;
        }

        public virtual void OnUpdate(List<Damageable> enemies)
        {
            enemies.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
            Damageable closestZombie = enemies.Count > 0 ? enemies[0] : null;
            float distanceToZombie = closestZombie ? Vector3.Distance(transform.position, closestZombie.transform.position) : 0;

            if (!_isMowing && closestZombie && distanceToZombie < _triggerRange) _isMowing = true;

            if (closestZombie && distanceToZombie < _damageRange)
            {
                closestZombie.TakeDamage(closestZombie.DamageableData.MaxHealth);
            }

            if (!_isMowing) return;
            transform.position += new Vector3(_moveSpeed * Time.deltaTime, 0);
            if (transform.position.x > GridUtilities.EndScreenWorldPosition)
            {
                Destroy(gameObject);
            }
        }
    }
}
