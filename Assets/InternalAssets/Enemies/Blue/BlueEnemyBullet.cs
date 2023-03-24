using InternalAssets.Player;
using UnityEngine;

namespace InternalAssets.Enemies.Blue
{
    public class BlueEnemyBullet : MonoBehaviour
    {
        [SerializeField] private BlueEnemy blueEnemy;
        private PlayerData _playerData;
        private Vector3 _defaultBulletPosition;

        private void Start()
        {
            _playerData = FindObjectOfType<PlayerData>();
            _defaultBulletPosition = new Vector3(1, 0, 0);
        }

        private void Update()
        {
            transform.position =
                    Vector3.MoveTowards(transform.position, _playerData.transform.position, 1f * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlayerData>() == null) return;
            _playerData.PowerPoints -= 25;
            transform.localPosition = _defaultBulletPosition;
            blueEnemy.bulletDeactivated?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
