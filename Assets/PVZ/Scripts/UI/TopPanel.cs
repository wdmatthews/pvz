using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace PVZ.UI
{
    [AddComponentMenu("PVZ/UI/Top Panel")]
    [DisallowMultipleComponent]
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private EventManagerSO _eventManager = null;

        [Space]
        [Header("Sun")]
        [SerializeField] private Image _sunIcon = null;
        [SerializeField] private TextMeshProUGUI _sunAmountLabel = null;
        [SerializeField] private float _sunAmountAnimationDuration = 0.25f;
        [SerializeField] private float _sunAmountAnimationPositiveScale = 1.1f;
        [SerializeField] private float _sunAmountAnimationNegativeScale = 1 / 1.1f;

        [Space]
        [Header("Seeds")]
        [SerializeField] private Transform _seedsContainer = null;
        [SerializeField] private SeedPacket _seedPacketPrefab = null;

        [Space]
        [Header("Shovel")]
        [SerializeField] private Button _shovelButton = null;
        [SerializeField] private Image _shovelButtonOutline = null;
        private bool _isShoveling = false;

        private void Awake()
        {
            _shovelButton.onClick.AddListener(ToggleShovel);
            _shovelButtonOutline.gameObject.SetActive(false);
            _eventManager.On("change-sun-amount", OnChangeSunAmount);
            _eventManager.On("add-seed-packet", OnAddSeedPacket);
            _eventManager.On("stop-shoveling", OnStopShoveling);
            _eventManager.On("enable-shoveling", OnEnableShoveling);
            _eventManager.On("disable-shoveling", OnDisableShoveling);
        }

        private void OnChangeSunAmount(int amount)
        {
            float change = amount - int.Parse(_sunAmountLabel.text);
            if (Mathf.Approximately(change, 0)) return;
            float sunIconScale = change > 0 ? _sunAmountAnimationPositiveScale : _sunAmountAnimationNegativeScale;
            _sunIcon.transform.DOScale(sunIconScale, _sunAmountAnimationDuration / 2)
                .OnComplete(() => _sunIcon.transform.DOScale(1, _sunAmountAnimationDuration / 2));
            _sunAmountLabel.text = $"{amount}";
        }

        private void OnAddSeedPacket(string name, Sprite icon, int cost, float cooldown)
        {
            SeedPacket seedPacket = Instantiate(_seedPacketPrefab, _seedsContainer);
            seedPacket.Initialize(name, icon, cost, cooldown, _eventManager);
        }

        private void SetShoveling(bool isShoveling)
        {
            _isShoveling = isShoveling;
            _shovelButtonOutline.gameObject.SetActive(_isShoveling);
        }

        private void ToggleShovel()
        {
            SetShoveling(!_isShoveling);
            _eventManager.Emit("toggle-shovel");
        }

        private void OnStopShoveling()
        {
            SetShoveling(false);
        }

        private void OnEnableShoveling()
        {
            _shovelButton.interactable = true;
        }

        private void OnDisableShoveling()
        {
            _shovelButton.interactable = false;
        }
    }
}
