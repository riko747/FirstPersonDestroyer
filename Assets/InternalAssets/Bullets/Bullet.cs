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
        private bool _ricochetedBullet;

        public Rigidbody RigidBody => rigidBody;

        private void OnEnable() => _ricochetedBullet = false;
        
        private void Start()
        {
            _playerData = FindObjectOfType<PlayerData>();
            _ricochetedBullet = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var percentageOfRicochet = 100 - _playerData.HealthPoints;
            var chanceOfRicochet = Random.Range(0, 101);
            if (!collision.gameObject.GetComponent<BlueEnemyData>() &&
                !collision.gameObject.GetComponent<RedEnemyData>()) return;
            
            if (_ricochetedBullet)
            {
                var randomBonus = Random.Range(0, 1);
                if (randomBonus == 0)
                    _playerData.PowerPoints += 10;
                else
                    _playerData.HealthPoints += 50;
            }
            
            if (chanceOfRicochet >= percentageOfRicochet)
                gameObject.SetActive(false);
            else
            {
                _ricochetedBullet = true;
                RigidBody.AddForce(transform.forward * Random.Range(5f, 10f), ForceMode.Impulse);
                StartCoroutine(DeactivateBullet());
            }
        }

        private IEnumerator DeactivateBullet()
        {
            yield return new WaitForSeconds(5f);
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.gameObject.SetActive(false);
            yield return null;
        }
    }
}