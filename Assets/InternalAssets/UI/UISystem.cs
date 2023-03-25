using System.Linq;
using DG.Tweening;
using InternalAssets.Enemies;
using InternalAssets.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InternalAssets.UI
{
    public class UISystem : MonoBehaviour
    {
        [SerializeField] private GameObject restartScreen;
        [SerializeField] private TextMeshProUGUI healthCount;
        [SerializeField] private TextMeshProUGUI powerCount;
        [SerializeField] private TextMeshProUGUI hitIndicator;
        [SerializeField] private TextMeshProUGUI scoreCount;
        [SerializeField] private Button ultimateButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button pauseButton;

        private PlayerData _playerData;
        private void Start()
        {
            Time.timeScale = 1;
            _playerData = FindObjectOfType<PlayerData>();
            UpdateStats();
            ultimateButton.onClick.AddListener(StartUltimate);
            restartButton.onClick.AddListener(RestartGame);
            pauseButton.onClick.AddListener(PauseGame);
        }

        public void HandleHit()
        {
            ShowHit();
            UpdateStats();
        }
        
        private void UpdateStats()
        {
            healthCount.text = _playerData.HealthPoints.ToString();
            powerCount.text = _playerData.PowerPoints.ToString();
            scoreCount.text = _playerData.Score.ToString();
            ultimateButton.gameObject.SetActive(_playerData.PowerPoints == 100);

            if (_playerData.HealthPoints != 0) return;
            Time.timeScale = 0;
            restartScreen.SetActive(true);
        }

        private void StartUltimate()
        {
            var enemies = FindObjectsOfType<Enemy>().Where(enemy => enemy.gameObject.activeSelf).ToList();
            foreach (var enemy in enemies)
                enemy.gameObject.SetActive(false);
            ultimateButton.gameObject.SetActive(false);
            _playerData.PowerPoints = 0;
            UpdateStats();
        }

        private void RestartGame() => SceneManager.LoadScene(0);

        private void PauseGame() => Time.timeScale = Time.timeScale == 1 ? 0 : 1;

        private void ShowHit()
        {
            var sequence = DOTween.Sequence();
            sequence.Kill();
            var fadeOut = hitIndicator.DOFade(1, 0);
            var fadeIn = hitIndicator.DOFade(0, 0.5f);
            hitIndicator.gameObject.SetActive(true);
            sequence.Append(fadeOut);
            sequence.Append(fadeIn);
            sequence.Play();
        }

        private void OnDestroy()
        {
            ultimateButton.onClick.RemoveListener(StartUltimate);
            restartButton.onClick.RemoveListener(RestartGame);
            pauseButton.onClick.RemoveListener(PauseGame);
        }
    }
}
