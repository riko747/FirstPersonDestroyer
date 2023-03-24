using System.Collections;
using System.Linq;
using DG.Tweening;
using InternalAssets.Bullets;
using InternalAssets.Player;
using InternalAssets.UI.Scripts;
using UnityEngine;

namespace InternalAssets.Enemies.Red
{
    public class RedEnemy : Enemy
    {
        [SerializeField] private RedEnemyData redEnemyData;

        private bool _followPlayer;
        private void Start()
        {
            PlayerData = FindObjectOfType<PlayerData>();
            UISystem = FindObjectOfType<UISystem>();
            EnemiesSpawner = FindObjectOfType<EnemiesSpawner>();
        }

        public void Attack() => StartCoroutine(AttackCoroutine());

        IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(1);
            _followPlayer = false;
            var sequence = DOTween.Sequence();
            var flyUp = transform.DOMoveY(CalculateHeight(), 4f);
            sequence.Append(flyUp);
            sequence.Play();
            sequence.onComplete += () => _followPlayer = true;
            yield return null;
        }

        private void Update()
        {
            if (!_followPlayer) return;
            
            if (transform.position.y <= 0.2f)
                gameObject.SetActive(false);
            else
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, PlayerData.transform.position, 1f * Time.deltaTime);
            }
        }

        private float CalculateHeight()
        {
            if (EnemiesSpawner.RedEnemies == null)
                return 4;
            return EnemiesSpawner.RedEnemies.Where(enemy => enemy.gameObject.activeSelf)
                .Max(enemy => enemy.transform.position.y);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Bullet>() != null)
            {
                redEnemyData.HealthPoints -= 50;
                if (redEnemyData.HealthPoints > 0) return;

                PlayerData.PowerPoints += 15;
                UISystem.HandleHit();
                gameObject.SetActive(false);
                PlayerData.Score += 1;
            }

            if (collision.gameObject.GetComponent<PlayerData>() == null) return;
            PlayerData.HealthPoints -= 15;
            UISystem.HandleHit();
            gameObject.SetActive(false);
        }
    }
}
