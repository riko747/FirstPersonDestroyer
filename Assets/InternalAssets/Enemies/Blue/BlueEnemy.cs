using InternalAssets.Bullets;
using InternalAssets.Player;
using InternalAssets.UI.Scripts;
using UnityEngine;

namespace InternalAssets.Enemies.Blue
{
    public class BlueEnemy : MonoBehaviour
    {
        [SerializeField] private BlueEnemyData blueEnemyData;
        private PlayerData _playerData;
        private UISystem _uiSystem;

        private void Start()
        {
            _playerData = FindObjectOfType<PlayerData>();
            _uiSystem = FindObjectOfType<UISystem>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Bullet>() == null) return;
            
            blueEnemyData.HealthPoints -= 50;
            if (blueEnemyData.HealthPoints > 0) return;
            gameObject.SetActive(false);
            _playerData.PowerPoints += 50;
            _uiSystem.HandleHit();
        }
    }
}
