using UnityEngine;

namespace PVZ.Plants
{
    [AddComponentMenu("PVZ/Plants/Plant")]
    [DisallowMultipleComponent]
    public class Plant : MonoBehaviour
    {
        private PlantSO _plantData = null;
        protected EventManagerSO _eventManager = null;

        public virtual void Place(PlantSO data, EventManagerSO eventManager)
        {
            _plantData = data;
            _eventManager = eventManager;
        }

        public virtual void OnUpdate()
        {

        }
    }
}
