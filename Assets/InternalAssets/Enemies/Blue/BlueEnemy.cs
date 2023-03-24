using System;
using System.Collections;
using InternalAssets.Bullets;
using InternalAssets.Player;
using InternalAssets.UI.Scripts;
using UnityEngine;

namespace InternalAssets.Enemies.Blue
{
    public class BlueEnemy : Enemy
    {
        [SerializeField] private BlueEnemyData blueEnemyData;
        [SerializeField] private GameObject enemyBullet;

        public Action bulletDeactivated;

        private void Start()
        {
            PlayerData = FindObjectOfType<PlayerData>();
            UISystem = FindObjectOfType<UISystem>();
            bulletDeactivated += () => StartCoroutine(ActivateBulletCoroutine());
        }

        public void Attack() => StartCoroutine(AttackCoroutine());

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(1);
            enemyBullet.SetActive(true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Bullet>() == null) return;
            
            blueEnemyData.HealthPoints -= 50;
            if (blueEnemyData.HealthPoints > 0) return;
            gameObject.SetActive(false);
            PlayerData.Score += 1;
            PlayerData.PowerPoints += 50;
            UISystem.HandleHit();
        }
        
        private IEnumerator ActivateBulletCoroutine()
        {
            yield return new WaitForSeconds(3);
            enemyBullet.SetActive(true);
        }
    }
}
