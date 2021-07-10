using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PVZ.UI
{
    [AddComponentMenu("PVZ/UI/Seed Packet")]
    [DisallowMultipleComponent]
    public class SeedPacket : MonoBehaviour
    {
        [SerializeField] private Button _selectButton = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _costLabel = null;
        [SerializeField] private Image _outline = null;
        [SerializeField] private Image _cooldownOverlay = null;

        private EventManagerSO _eventManager = null;
        private bool _isSelected = false;
        private string _name = "";
        private int _cost = 0;
        private Timer _cooldownTimer = null;

        private void Update()
        {
            if (_cooldownTimer.IsRunning)
            {
                _cooldownTimer.Tick();
                _cooldownOverlay.fillAmount = _cooldownTimer.TimePercentage;
            }
        }

        public void Initialize(string name, Sprite icon, int cost, float cooldown, EventManagerSO eventManager)
        {
            _eventManager = eventManager;
            _name = name;
            _cost = cost;
            _cooldownTimer = new Timer(cooldown, OnCooldownDone, false);
            _icon.sprite = icon;
            _costLabel.text = $"{_cost}";
            _selectButton.onClick.AddListener(OnToggleSelect);
            _outline.gameObject.SetActive(false);
            _cooldownOverlay.gameObject.SetActive(false);
            _eventManager.On("select-seed", OnOtherSelect);
            _eventManager.On("plant-seed", OnPlantSeed);
            _eventManager.On("change-sun-amount", OnChangeSunAmount);
        }

        private void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            _outline.gameObject.SetActive(_isSelected);
        }

        private void OnToggleSelect()
        {
            SetSelected(!_isSelected);
            _eventManager.Emit("select-seed", _name);
        }

        private void OnOtherSelect(string name)
        {
            if (name != _name) SetSelected(false);
        }

        private void OnPlantSeed(string name)
        {
            if (name != _name) return;
            SetSelected(false);
            _selectButton.interactable = false;
            _cooldownTimer.Reset();
            _cooldownTimer.Start();
            _cooldownOverlay.gameObject.SetActive(true);
            _cooldownOverlay.fillAmount = 1;
        }

        private void OnChangeSunAmount(int amount)
        {
            if (_cooldownTimer.IsRunning) return;
            _selectButton.interactable = amount >= _cost;
        }

        private void OnCooldownDone()
        {
            _cooldownOverlay.gameObject.SetActive(false);
            _eventManager.Emit("seed-timer-done");
        }
    }
}
