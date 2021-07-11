using System.Collections.Generic;
using UnityEngine;
using PVZ.Combat;

namespace PVZ.Zombies
{
    [AddComponentMenu("PVZ/Zombies/Zombie Manager")]
    [DisallowMultipleComponent]
    public class ZombieManager : MonoBehaviour
    {
        private const int spawnPosition = 11;

        [SerializeField] private EventManagerSO _zombieEventManager = null;
        [SerializeField] private EventManagerSO _combatEventManager = null;
        [SerializeField] private EventManagerSO _uiEventManager = null;
        [SerializeField] private ZombieSO[] _zombieSOs = { };

        private Dictionary<string, ZombieSO> _zombieSOsByName = new Dictionary<string, ZombieSO>();
        private List<Zombie> _zombies = new List<Zombie>();
        private List<Damageable> _plants = new List<Damageable>();

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
            SpawnZombie("Zombie", new Vector2Int(spawnPosition, Random.Range(0, 5)));
        }

        private void Update()
        {
            for (int i = _zombies.Count - 1; i >= 0; i--)
            {
                Zombie zombie = _zombies[i];
                zombie.OnUpdate(_plants.FindAll(plant => plant.Position.y == zombie.Position.y));
            }
        }

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
