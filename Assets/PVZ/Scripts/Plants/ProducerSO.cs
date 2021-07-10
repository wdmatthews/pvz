using UnityEngine;

namespace PVZ.Plants
{
    [CreateAssetMenu(fileName = "New Producer", menuName = "PVZ/Plants/Producer")]
    public class ProducerSO : PlantSO
    {
        public float ProductionCooldown = 1;
        public int ProductionAmountPerCooldown = 10;
    }
}
