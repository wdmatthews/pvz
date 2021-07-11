using UnityEngine;

namespace PVZ.Combat
{
    [AddComponentMenu("PVZ/Combat/Damageable")]
    [DisallowMultipleComponent]
    public class Damageable : MonoBehaviour
    {
        private DamageableSO _damageableData = null;
        public DamageableSO DamageableData {
            get => _damageableData;
            set
            {
                _damageableData = value;
                _health = value.MaxHealth;
            }
        }
        private int _health = 0;
        protected EventManagerSO _combatEventManager = null;

        protected void TakeDamage(int amount)
        {
            _health = Mathf.Clamp(_health - amount, 0, _damageableData.MaxHealth);
            if (_health == 0) _combatEventManager.Emit("damageable-died", this);
        }
    }
}
