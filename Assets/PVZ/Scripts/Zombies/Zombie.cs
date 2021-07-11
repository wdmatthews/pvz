using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Zombies
{
    [AddComponentMenu("PVZ/Zombies/Zombie")]
    [DisallowMultipleComponent]
    public class Zombie : Damageable
    {
        private ZombieSO _zombieData = null;
        protected EventManagerSO _zombieEventManager = null;
        private Timer _attackTimer = null;
        private Damageable _closestPlant = null;

        public virtual void Spawn(ZombieSO data, Vector2Int position, EventManagerSO zombieEventManager, EventManagerSO combatEventManager)
        {
            _zombieData = data;
            DamageableData = _zombieData;
            Position = position;
            _zombieEventManager = zombieEventManager;
            _combatEventManager = combatEventManager;
            _attackTimer = new Timer(_zombieData.AttackCooldown, null, false);
        }

        public override void OnUpdate(List<Damageable> enemies)
        {
            base.OnUpdate(enemies);
            enemies.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
            Damageable closestPlant = enemies.Count > 0 ? enemies[0] : null;

            if (closestPlant && Vector3.Distance(transform.position, closestPlant.transform.position) < _zombieData.Range)
            {
                if (closestPlant && !_closestPlant)
                {
                    _attackTimer.Reset();
                    _attackTimer.Start();
                }
                else _attackTimer.Tick();

                if (!_attackTimer.IsRunning)
                {
                    _attackTimer.Start();
                    closestPlant.TakeDamage(DamageableData.Damage);
                }
            }
            else transform.position -= new Vector3(_zombieData.MoveSpeed * Time.deltaTime, 0);

            _closestPlant = closestPlant;
        }
    }
}
