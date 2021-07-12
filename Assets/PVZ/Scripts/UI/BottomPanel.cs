using UnityEngine;
using TMPro;
using DG.Tweening;

namespace PVZ.UI
{
    [AddComponentMenu("PVZ/UI/Bottom Panel")]
    [DisallowMultipleComponent]
    public class BottomPanel : MonoBehaviour
    {
        [SerializeField] private EventManagerSO _eventManager = null;

        [Space]
        [Header("Zombies Are Coming")]
        [SerializeField] private TextMeshProUGUI _warningLabel = null;
        [SerializeField] private float _warningAnimationDuration = 3;

        private void Awake()
        {
            _eventManager.On("zombies-are-coming", OnZombiesAreComing);
        }

        private void Start()
        {
            _warningLabel.gameObject.SetActive(false);
        }

        private void OnZombiesAreComing()
        {
            _warningLabel.gameObject.SetActive(true);
            Sequence animation = DOTween.Sequence();
            animation.Append(DOTween.ToAlpha(() => _warningLabel.color,
                c => _warningLabel.color = c, 1, _warningAnimationDuration / 3).From(0));
            animation.AppendInterval(_warningAnimationDuration / 3);
            animation.Append(DOTween.ToAlpha(() => _warningLabel.color,
                c => _warningLabel.color = c, 0, _warningAnimationDuration / 3));
            animation.Play();
        }
    }
}
