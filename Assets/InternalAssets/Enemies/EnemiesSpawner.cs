using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Enemies.Blue;
using InternalAssets.Enemies.Red;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Enemies
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField] private BlueEnemy blueEnemy;
        [SerializeField] private RedEnemy redEnemy;
        [SerializeField] private GameObject arena;

        private List<RedEnemy> _redEnemies = new();
        private List<BlueEnemy> _blueEnemies = new();

        private float _spawnTime = 5;

        private void Start()
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        private IEnumerator SpawnEnemiesCoroutine()
        {
            while (true)
            {
                if (_redEnemies.Count(b => !b.gameObject.activeSelf) > 0 && _redEnemies.Count != 0)
                {
                    redEnemy = _redEnemies.FirstOrDefault(b => !b.gameObject.activeSelf && redEnemy);
                    if (redEnemy)
                    {
                        redEnemy.gameObject.SetActive(true);
                        redEnemy.transform.position = CalculateSpawnPositionAndSpawnEnemy();;
                    }
                }
                else
                {
                    redEnemy = Instantiate(redEnemy, transform, true);
                    redEnemy.transform.position = CalculateSpawnPositionAndSpawnEnemy();;
                    _redEnemies.Add(redEnemy);
                }

                if (_blueEnemies.Count(b => !b.gameObject.activeSelf) > 0 && _blueEnemies.Count != 0)
                {
                    blueEnemy = _blueEnemies.FirstOrDefault(b => !b.gameObject.activeSelf && blueEnemy);
                    if (blueEnemy)
                    {
                        blueEnemy.gameObject.SetActive(true);
                        blueEnemy.transform.position = CalculateSpawnPositionAndSpawnEnemy();;
                    }
                }
                else
                {
                    blueEnemy = Instantiate(blueEnemy, transform, true);
                    blueEnemy.transform.position = CalculateSpawnPositionAndSpawnEnemy();;
                    _blueEnemies.Add(blueEnemy);
                }

                _spawnTime = Mathf.Max(2f, _spawnTime - 0.5f);
                yield return new WaitForSeconds(_spawnTime);
            }
        }

        private Vector3 CalculateSpawnPositionAndSpawnEnemy()
        {
            while (true)
            {
                var childGameObjects = arena.transform.Cast<Transform>()
                    .Select(child => child.gameObject);
                var newX = Random.Range(-4.5f, 4.5f);
                var newZ = Random.Range(-4.5f, 4.5f);
                var position = new Vector3(newX, 0.5f, newZ);
                var positionIsOnAnyArenaColumns = childGameObjects.Any(child =>
                    Vector3.Distance(position, child.transform.position) < 1);
                if (!positionIsOnAnyArenaColumns)
                {
                    return position;
                }
            }
        }
    }
}
