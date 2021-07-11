using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    public class Producer : Plant
    {
        private ProducerSO _producerData = null;
        private Timer _productionTimer = null;

        public override void Place(PlantSO data, Vector2Int position,
            EventManagerSO plantEventManager, EventManagerSO combatEventManager)
        {
            base.Place(data, position, plantEventManager, combatEventManager);
            _producerData = (ProducerSO)data;
            _productionTimer = new Timer(_producerData.ProductionCooldown, OnProduce);
            _productionTimer.Reset(_producerData.ProductionCooldown);
        }

        public override void OnUpdate(List<Damageable> enemies)
        {
            base.OnUpdate(enemies);
            if (_productionTimer == null) return;
            _productionTimer.Tick();
        }

        private void OnProduce()
        {
            _plantEventManager.Emit("produce-sun", _producerData.ProductionAmountPerCooldown);
        }
    }
}
