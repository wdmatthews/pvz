using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    [AddComponentMenu("PVZ/Plants/Plant")]
    [DisallowMultipleComponent]
    public class Plant : Damageable
    {
        private PlantSO _plantData = null;
        protected EventManagerSO _plantEventManager = null;
        public Vector2Int Position { get; private set; }

        public virtual void Place(PlantSO data, Vector2Int position, EventManagerSO plantEventManager, EventManagerSO combatEventManager)
        {
            _plantData = data;
            DamageableData = _plantData;
            Position = position;
            _plantEventManager = plantEventManager;
            _combatEventManager = combatEventManager;
        }

        public virtual void OnUpdate()
        {

        }
    }
}
