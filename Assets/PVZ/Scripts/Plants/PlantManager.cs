using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Plants
{
    [AddComponentMenu("PVZ/Plants/Plant Manager")]
    [DisallowMultipleComponent]
    public class PlantManager : MonoBehaviour
    {
        private const int _maxSunAmount = 10000;

        [SerializeField] private EventManagerSO _plantEventManager = null;
        [SerializeField] private EventManagerSO _combatEventManager = null;
        [SerializeField] private EventManagerSO _uiEventManager = null;
        [SerializeField] private SpriteRenderer _tileCursor = null;
        [SerializeField] private PlantSO[] _plantSOs = { };
        [SerializeField] private ProjectileSO[] _plantProjectileSOs = { };

        private Dictionary<string, PlantSO> _plantSOsByName = new Dictionary<string, PlantSO>();
        private List<Plant> _plants = new List<Plant>();
        private Dictionary<Vector2Int, Plant> _plantsByPosition = new Dictionary<Vector2Int, Plant>();
        private int _sunAmount = 50;
        private string _selectedPlantPacket = "";
        private bool _isShoveling = false;
        private List<Damageable> _zombies = new List<Damageable>();
        private Dictionary<string, ProjectileSO> _plantProjectileSOsByName = new Dictionary<string, ProjectileSO>();
        private List<Projectile> _projectiles = new List<Projectile>();

        private void Awake()
        {
            foreach (var plantSO in _plantSOs)
            {
                _plantSOsByName.Add(plantSO.name, plantSO);
            }

            foreach (var projectileSO in _plantProjectileSOs)
            {
                _plantProjectileSOsByName.Add(projectileSO.name, projectileSO);
            }

            _plantEventManager.On("produce-sun", ChangeSunAmount);
            _combatEventManager.On("damageable-died", OnDamageableDied);
            _combatEventManager.On("spawn-zombie", OnSpawnZombie);
            _combatEventManager.On("plant-attack", OnPlantAttack);
            _uiEventManager.On("select-seed", SelectSeed);
            _uiEventManager.On("seed-timer-done", OnSeedTimerDone);
            _uiEventManager.On("toggle-shovel", OnToggleShovel);
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
            for (int i = _plants.Count - 1; i >= 0; i--) {
                Plant plant = _plants[i];
                plant.OnUpdate(_zombies.FindAll(zombie => zombie.Position.y == plant.Position.y));
            }
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = _projectiles[i];
                if (!projectile)
                {
                    _projectiles.RemoveAt(i);
                    continue;
                }
                projectile.OnUpdate(_zombies.FindAll(zombie => zombie.Position.y
                    == GridUtilities.WorldToGrid(projectile.transform.position).y));
            }

            if (_selectedPlantPacket != "" || _isShoveling)
            {
                Vector2Int mouseGridPosition = MouseUtilities.GridPosition;
                Vector3 mouseWorldPosition = GridUtilities.GridToWorld(mouseGridPosition);
                _tileCursor.transform.position = mouseWorldPosition;

                if (MouseUtilities.IsPressed && GridUtilities.PointIsInGrid(mouseGridPosition))
                {
                    if (_isShoveling)
                    {
                        RemovePlant(mouseGridPosition);
                        _uiEventManager.Emit("stop-shoveling");
                    }
                    else if (!_plantsByPosition.ContainsKey(mouseGridPosition))
                    {
                        PlacePlant(_selectedPlantPacket, mouseGridPosition);
                        _uiEventManager.Emit("plant-seed", _selectedPlantPacket);
                        SelectSeed("");
                    }
                }
            }
        }

        private void SelectSeed(string name)
        {
            _selectedPlantPacket = _selectedPlantPacket == name ? "" : name;
            _tileCursor.gameObject.SetActive(_selectedPlantPacket != "");
            _uiEventManager.Emit($"{(_selectedPlantPacket == "" ? "enable" : "disable")}-shoveling");
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
            _plants.Add(plant);
            _plantsByPosition.Add(position, plant);
            plant.transform.position = GridUtilities.GridToWorld(position);
            plant.Place(plantSO, position, _plantEventManager, _combatEventManager);
            ChangeSunAmount(-plantSO.Cost);
            _combatEventManager.Emit("place-plant", plant);
        }

        private void RemovePlant(Vector2Int position)
        {
            if (_isShoveling)
            {
                _tileCursor.gameObject.SetActive(false);
                _uiEventManager.Emit("change-sun-amount", _sunAmount);
                _isShoveling = false;
            }
            if (!_plantsByPosition.ContainsKey(position)) return;
            Plant plant = _plantsByPosition[position];
            _plants.Remove(plant);
            _plantsByPosition.Remove(position);
            Destroy(plant.gameObject);
        }

        private void OnSeedTimerDone()
        {
            _uiEventManager.Emit("change-sun-amount", _sunAmount);
        }

        private void OnToggleShovel()
        {
            _isShoveling = !_isShoveling;
            _tileCursor.gameObject.SetActive(_isShoveling);
            if (_isShoveling) _uiEventManager.Emit("is-shoveling");
            else _uiEventManager.Emit("change-sun-amount", _sunAmount);
        }

        private void OnDamageableDied(MonoBehaviour damageableMonoBehaviour)
        {
            Damageable damageable = (Damageable)damageableMonoBehaviour;
            if (!damageable.DamageableData.IsPlant)
            {
                _zombies.Remove(damageable);
                return;
            }
            Plant plant = (Plant)damageable;
            RemovePlant(plant.Position);
        }

        private void OnSpawnZombie(MonoBehaviour zombie)
        {
            _zombies.Add((Damageable)zombie);
        }

        private void OnPlantAttack(Vector3 position, string projectileName)
        {
            ProjectileSO projectileSO = _plantProjectileSOsByName[projectileName];
            Projectile projectile = Instantiate(projectileSO.Prefab, transform);
            projectile.Spawn(projectileSO, position);
            _projectiles.Add(projectile);
        }
    }
}
