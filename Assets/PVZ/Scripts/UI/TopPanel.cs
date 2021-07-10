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

        private void Awake()
        {
            _eventManager.On("sun-amount-change", OnSunAmountChange);
        }

        private void OnSunAmountChange(int amount)
        {
            float sunIconScale = amount - int.Parse(_sunAmountLabel.text) >= 0
                ? _sunAmountAnimationPositiveScale : _sunAmountAnimationNegativeScale;
            _sunIcon.transform.DOScale(sunIconScale, _sunAmountAnimationDuration / 2)
                .OnComplete(() => _sunIcon.transform.DOScale(1, _sunAmountAnimationDuration / 2));
            _sunAmountLabel.text = $"{amount}";
        }

        public void TestOnSunAmountChange()
        {
            _eventManager.Emit("sun-amount-change", Random.Range(0, 11));
        }
    }
}
