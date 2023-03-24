using InternalAssets.Player;
using InternalAssets.UI.Scripts;
using UnityEngine;

namespace InternalAssets.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private PlayerData _playerData;
        private UISystem _uiSystem;
        private EnemiesSpawner _enemiesSpawner;

        protected PlayerData PlayerData { get; set; }
        protected UISystem UISystem { get; set; }
        
        protected EnemiesSpawner EnemiesSpawner { get; set; }
    }
}
