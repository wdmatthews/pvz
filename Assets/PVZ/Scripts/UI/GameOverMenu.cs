using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PVZ.UI
{
    [AddComponentMenu("PVZ/UI/Game Over Menu")]
    [DisallowMultipleComponent]
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private Button _retryButton = null;

        private void Awake()
        {
            _retryButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        }
    }
}
