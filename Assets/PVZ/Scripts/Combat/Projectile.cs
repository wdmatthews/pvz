using System.Collections.Generic;
using UnityEngine;

namespace PVZ.Combat
{
    [AddComponentMenu("PVZ/Combat/Projectile")]
    [DisallowMultipleComponent]
    public class Projectile : MonoBehaviour
    {
        private ProjectileSO _projectileData = null;
        private Vector3 _spawnPosition = new Vector3();

        public virtual void Spawn(ProjectileSO projectileData, Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            _projectileData = projectileData;
            _spawnPosition = spawnPosition;
        }

        public virtual void OnUpdate(List<Damageable> enemies)
        {
            enemies.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
            Damageable closestZombie = enemies.Count > 0 ? enemies[0] : null;

            if (closestZombie && Vector3.Distance(transform.position, closestZombie.transform.position) < _projectileData.DamageRange)
            {
                closestZombie.TakeDamage(_projectileData.Damage);
                Destroy(gameObject);
            }
            else
            {
                transform.position += new Vector3(_projectileData.MoveSpeed * Time.deltaTime, 0);
                if (transform.position.x - _spawnPosition.x > _projectileData.AttackRange)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
