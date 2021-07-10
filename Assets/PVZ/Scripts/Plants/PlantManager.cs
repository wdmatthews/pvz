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
        [SerializeField] private SpriteRenderer _tileCursor = null;
        [SerializeField] private PlantSO[] _plantSOs = { };

        private Dictionary<string, PlantSO> _plantSOsByName = new Dictionary<string, PlantSO>();
        private Dictionary<Vector2Int, Plant> _plantsByPosition = new Dictionary<Vector2Int, Plant>();
        private int _sunAmount = 50;
        private string _selectedPlantPacket = "";

        private void Awake()
        {
            foreach (var plantSO in _plantSOs)
            {
                _plantSOsByName.Add(plantSO.name, plantSO);
            }

            _uiEventManager.On("select-seed", SelectSeed);
            _uiEventManager.On("seed-timer-done", OnSeedTimerDone);
            _plantEventManager.On("produce-sun", ChangeSunAmount);
            _tileCursor.gameObject.SetActive(false);
        }

        private void Start()
        {
            foreach (var plantSO in _plantSOs)
            {
                _uiEventManager.Emit("add-seed-packet", plantSO.name, plantSO.Icon, plantSO.Cost, plantSO.PlaceCooldown);
            }

            _uiEventManager.Emit("change-sun-amount", _sunAmount);
        }

        private void Update()
        {
            foreach (var plant in _plantsByPosition)
            {
                plant.Value.OnUpdate();
            }

            if (_selectedPlantPacket != "")
            {
                Vector2Int mouseGridPosition = MouseUtilities.GridPosition;
                Vector3 mouseWorldPosition = GridUtilities.GridToWorld(mouseGridPosition);
                _tileCursor.transform.position = mouseWorldPosition;

                if (MouseUtilities.IsPressed && GridUtilities.PointIsInGrid(mouseGridPosition)
                    && !_plantsByPosition.ContainsKey(mouseGridPosition))
                {
                    PlacePlant(_selectedPlantPacket, mouseGridPosition);
                    _uiEventManager.Emit("plant-seed", _selectedPlantPacket);
                    SelectSeed("");
                }
            }
        }

        private void SelectSeed(string name)
        {
            _selectedPlantPacket = _selectedPlantPacket == name ? "" : name;
            _tileCursor.gameObject.SetActive(_selectedPlantPacket != "");
        }

        private void ChangeSunAmount(int amount)
        {
            _sunAmount = Mathf.Clamp(_sunAmount + amount, 0, _maxSunAmount);
            _uiEventManager.Emit("change-sun-amount", _sunAmount);
        }

        private void PlacePlant(string name, Vector2Int position)
        {
            PlantSO plantSO = _plantSOsByName[name];
            Plant plant = Instantiate(plantSO.Prefab, transform);
            _plantsByPosition.Add(position, plant);
            plant.transform.position = GridUtilities.GridToWorld(position);
            plant.Place(plantSO, _plantEventManager);
            ChangeSunAmount(-plantSO.Cost);
        }

        private void OnSeedTimerDone()
        {
            _uiEventManager.Emit("change-sun-amount", _sunAmount);
        }
    }
}
