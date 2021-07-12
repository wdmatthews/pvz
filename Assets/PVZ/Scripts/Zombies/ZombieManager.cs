using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Zombies
{
    [AddComponentMenu("PVZ/Zombies/Zombie Manager")]
    [DisallowMultipleComponent]
    public class ZombieManager : MonoBehaviour
    {
        private const int _spawnPosition = 11;
        private const float _startTime = 20;
        private const float _maxSpawnTime = 20;
        private const float _minSpawnTime = 3;
        private const float _spawnTimeChange = 0.5f;

        [SerializeField] private EventManagerSO _zombieEventManager = null;
        [SerializeField] private EventManagerSO _combatEventManager = null;
        [SerializeField] private EventManagerSO _uiEventManager = null;
        [SerializeField] private ZombieSO[] _zombieSOs = { };

        private Dictionary<string, ZombieSO> _zombieSOsByName = new Dictionary<string, ZombieSO>();
        private List<Zombie> _zombies = new List<Zombie>();
        private List<Damageable> _plants = new List<Damageable>();
        private Timer _startTimer = null;
        private Timer _spawnTimer = null;
        private bool _wasStarted = false;
        private float _spawnTime = 0;

        private void Awake()
        {
            foreach (var zombieSO in _zombieSOs)
            {
                _zombieSOsByName.Add(zombieSO.name, zombieSO);
            }

            _combatEventManager.On("damageable-died", OnDamageableDied);
            _combatEventManager.On("place-plant", OnPlacePlant);
        }

        private void Start()
        {
            _spawnTime = _maxSpawnTime;
            _startTimer = new Timer(_startTime, OnStart, false);
            _startTimer.Start();
            _spawnTimer = new Timer(_spawnTime, OnSpawn, false);
        }

        private void Update()
        {
            for (int i = _zombies.Count - 1; i >= 0; i--)
            {
                Zombie zombie = _zombies[i];
                zombie.OnUpdate(_plants.FindAll(plant => plant.Position.y == zombie.Position.y));
            }

            if (_wasStarted) _spawnTimer.Tick();
            else _startTimer.Tick();
        }

        private void OnStart()
        {
            _wasStarted = true;
            _uiEventManager.Emit("zombies-are-coming");
            SpawnZombie();
            _spawnTimer.Start();
        }

        private void OnSpawn()
        {
            SpawnZombie();
            _spawnTime = Mathf.Clamp(_spawnTime - _spawnTimeChange, _minSpawnTime, _maxSpawnTime);
            _spawnTimer.Reset(_spawnTime);
            _spawnTimer.Start();
        }

        private void SpawnZombie() => SpawnZombie("Zombie", new Vector2Int(_spawnPosition, Random.Range(0, 5)));

        private void SpawnZombie(string name, Vector2Int position)
        {
            ZombieSO zombieSO = _zombieSOsByName[name];
            Zombie zombie = Instantiate(zombieSO.Prefab, transform);
            _zombies.Add(zombie);
            zombie.transform.position = GridUtilities.GridToWorld(position);
            zombie.Spawn(zombieSO, position, _zombieEventManager, _combatEventManager);
            _combatEventManager.Emit("spawn-zombie", zombie);
        }

        private void OnDamageableDied(MonoBehaviour damageableMonoBehaviour)
        {
            Damageable damageable = (Damageable)damageableMonoBehaviour;
            if (damageable.DamageableData.IsPlant)
            {
                _plants.Remove(damageable);
                return;
            }
            Zombie zombie = (Zombie)damageable;
            _zombies.Remove(zombie);
            Destroy(zombie.gameObject);
        }

        private void OnPlacePlant(MonoBehaviour plant)
        {
            _plants.Add((Damageable)plant);
        }
    }
}
