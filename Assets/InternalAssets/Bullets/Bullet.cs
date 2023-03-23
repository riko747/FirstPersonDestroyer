using System;
using System.Collections;
using InternalAssets.Enemies.Blue;
using InternalAssets.Enemies.Red;
using InternalAssets.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        
        private PlayerData _playerData;

        public Rigidbody RigidBody => rigidBody;

        private void Start()
        {
            _playerData = FindObjectOfType<PlayerData>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var percentageOfRicochet = 100 - _playerData.HealthPoints;
            var chanceOfRicochet = Random.Range(0, 101);
            if (!collision.gameObject.GetComponent<BlueEnemyData>() &&
                !collision.gameObject.GetComponent<RedEnemyData>()) return;
            
            RigidBody.AddForce(transform.forward * Random.Range(5f, 10f), ForceMode.Impulse);
            if (chanceOfRicochet >= percentageOfRicochet)
                gameObject.SetActive(false);
        }

        private IEnumerator WitForRicochetCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}