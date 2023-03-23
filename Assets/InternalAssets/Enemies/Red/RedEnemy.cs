using InternalAssets.Bullets;
using InternalAssets.Player;
using InternalAssets.UI.Scripts;
using UnityEngine;

namespace InternalAssets.Enemies.Red
{
    public class RedEnemy : MonoBehaviour
    {
        [SerializeField] private RedEnemyData redEnemyData;
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
            
            redEnemyData.HealthPoints -= 50;
            if (redEnemyData.HealthPoints > 0) return;
            
            gameObject.SetActive(false);
            _playerData.PowerPoints += 15;
            _uiSystem.HandleHit();
        }
    }
}
