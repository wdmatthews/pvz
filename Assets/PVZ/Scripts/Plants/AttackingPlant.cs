using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    public class AttackingPlant : Plant
    {
        private AttackingPlantSO _attackData = null;
        private Timer _attackTimer = null;
        private Damageable _targetZombie = null;

        public override void Place(PlantSO data, Vector2Int position,
            EventManagerSO plantEventManager, EventManagerSO combatEventManager)
        {
            base.Place(data, position, plantEventManager, combatEventManager);
            _attackData = (AttackingPlantSO)data;
            _attackTimer = new Timer(_attackData.AttackCooldown, OnAttack, false);
        }

        public override void OnUpdate(List<Damageable> enemies)
        {
            base.OnUpdate(enemies);
            if (_attackTimer == null) return;
            enemies.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
            Damageable closestZombie = enemies.Count > 0 ? enemies[0] : null;
            if (!closestZombie)
            {
                _targetZombie = null;
                _attackTimer.Stop();
            }

            if (closestZombie && !_targetZombie
                && Vector3.Distance(transform.position, closestZombie.transform.position) < _attackData.Projectile.AttackRange)
            {
                _attackTimer.Reset();
                _attackTimer.Start();
                _targetZombie = closestZombie;
            }
            else
            {
                _attackTimer.Tick();
                if (_targetZombie && !_attackTimer.IsRunning)
                {
                    _attackTimer.Reset();
                    _attackTimer.Start();
                }
            }
        }

        protected void OnAttack()
        {
            _combatEventManager.Emit("plant-attack", transform.position, _attackData.Projectile.name);
        }
    }
}
