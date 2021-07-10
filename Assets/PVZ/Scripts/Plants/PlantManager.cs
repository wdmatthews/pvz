using System.Collections.Generic;
using UnityEngine;

namespace PVZ.Plants
{
    [AddComponentMenu("PVZ/Plants/Plant Manager")]
    [DisallowMultipleComponent]
    public class PlantManager : MonoBehaviour
    {
        private const int _maxSunAmount = 10000;

        [SerializeField] private EventManagerSO _plantEventManager = null;
        [SerializeField] private EventManagerSO _uiEventManager = null;
        [SerializeField] private PlantSO[] _plantSOs = { };

        private Dictionary<string, PlantSO> _plantSOsByName = new Dictionary<string, PlantSO>();
        private Dictionary<Vector2Int, Plant> _plantsByPosition = new Dictionary<Vector2Int, Plant>();
        private int _sunAmount = 50;

        private void Awake()
        {
            foreach (var plantSO in _plantSOs)
            {
                _plantSOsByName.Add(plantSO.name, plantSO);
            }

            _plantEventManager.On("produce-sun", ChangeSunAmount);
        }

        private void Start()
        {
            _uiEventManager.Emit("sun-amount-change", _sunAmount);
            PlacePlant("Producer", new Vector2Int(0, 0));
        }

        private void Update()
        {
            foreach (var plant in _plantsByPosition)
            {
                plant.Value.OnUpdate();
            }
        }

        private void PlacePlant(string plantName, Vector2Int position)
        {
            PlantSO plantSO = _plantSOsByName[plantName];
            Plant plant = Instantiate(plantSO.Prefab, transform);
            _plantsByPosition.Add(position, plant);
            plant.transform.position = GridUtilities.CellToWorld(position);
            plant.Place(plantSO, _plantEventManager);
            ChangeSunAmount(-plantSO.SunCost);
        }

        private void ChangeSunAmount(int amount)
        {
            _sunAmount = Mathf.Clamp(_sunAmount + amount, 0, _maxSunAmount);
            _uiEventManager.Emit("sun-amount-change", _sunAmount);
        }
    }
}
