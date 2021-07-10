namespace PVZ.Plants
{
    public class Producer : Plant
    {
        private ProducerSO _producerData = null;
        private Timer _productionTimer = null;

        public override void Place(PlantSO data, EventManagerSO eventManager)
        {
            base.Place(data, eventManager);
            _producerData = (ProducerSO)data;
            _productionTimer = new Timer(_producerData.ProductionCooldown, OnProduce);
            _productionTimer.Reset(_producerData.ProductionCooldown);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _productionTimer.Tick();
        }

        private void OnProduce()
        {
            _eventManager.Emit("produce-sun", _producerData.ProductionAmountPerCooldown);
        }
    }
}
