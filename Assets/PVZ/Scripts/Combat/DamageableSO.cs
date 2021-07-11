using UnityEngine;

namespace PVZ.Combat
{
    [CreateAssetMenu(fileName = "New Damageable", menuName = "PVZ/Combat/Damageable")]
    public class DamageableSO : ScriptableObject
    {
        public bool IsPlant = true;
        public int MaxHealth = 20;
    }
}
