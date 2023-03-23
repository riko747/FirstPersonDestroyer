using DG.Tweening;
using InternalAssets.Player;
using TMPro;
using UnityEngine;

namespace InternalAssets.UI.Scripts
{
    public class UISystem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthCount;
        [SerializeField] private TextMeshProUGUI powerCount;
        [SerializeField] private TextMeshProUGUI hitIndicator;

        private PlayerData playerData;
        private void Start()
        {
            playerData = FindObjectOfType<PlayerData>();
            UpdateStats();
        }

        public void HandleHit()
        {
            ShowHit();
            UpdateStats();
        }
        private void UpdateStats()
        {
            healthCount.text = playerData.HealthPoints.ToString();
            powerCount.text = playerData.PowerPoints.ToString();
        }

        private void ShowHit()
        {
            var sequence = DOTween.Sequence();
            ClearTween(sequence);
            var fadeOut = hitIndicator.DOFade(1, 0);
            var fadeIn = hitIndicator.DOFade(0, 0.5f);
            hitIndicator.gameObject.SetActive(true);
            sequence.Append(fadeOut);
            sequence.Append(fadeIn);
            sequence.Play();
        }

        private void ClearTween(Sequence sequence)
        {
            sequence.Kill();
        }
    }
}
