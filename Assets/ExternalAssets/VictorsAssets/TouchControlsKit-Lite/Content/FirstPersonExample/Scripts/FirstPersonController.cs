using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Bullets;
using InternalAssets.Enemies;
using TouchControlsKit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExternalAssets.VictorsAssets.TouchControlsKit_Lite.Content.FirstPersonExample.Scripts
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private Transform bulletParentTransform;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Bullet bullet;
        [SerializeField] private CharacterController characterController;

        private List<Bullet> _bullets = new();

        private Transform _myTransform;
        private bool _bind;
        private float _rotation;
        private bool _prevGrounded;
        private float _weaponReadyTime;
        private bool _weaponReady = true;
        private float _nearEnemiesRadius = 3;
        public float NearEnemiesRadius
        {
            get => _nearEnemiesRadius;
            set => _nearEnemiesRadius = Mathf.Clamp(value, 0.1f, 3);
        }

        private void Awake()
        {
            _myTransform = transform;
            NearEnemiesRadius = 3f;
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            HandleWeaponReady();
            HandlePlayerInput();
            HandlePlayerMovement();
        }

        private void HandleWeaponReady()
        {
            if (_weaponReady) return;
            _weaponReadyTime += Time.deltaTime;
            if (!(_weaponReadyTime > .15f)) return;
            _weaponReady = true;
            _weaponReadyTime = 0f;
        }

        private void HandlePlayerInput()
        {
            if (TCKInput.GetAction("fireBtn", EActionEvent.Press))
                PlayerFiring();

            var look = TCKInput.GetAxis("RotationJoystick");
            PlayerRotation(look.x / 2, look.y / 2);
        }

        private void HandlePlayerMovement()
        {
            var move = TCKInput.GetAxis("MovementJoystick");
            var grounded = characterController.isGrounded;
            var moveDirection = _myTransform.forward * move.y;
            moveDirection += _myTransform.right * move.x;

            if (grounded)
                moveDirection *= 3f;

            characterController.Move(moveDirection * Time.fixedDeltaTime);

            if (!_prevGrounded && grounded)
                moveDirection.y = 0f;

            _prevGrounded = grounded;
        }

        private void PlayerRotation(float horizontal, float vertical)
        {
            _myTransform.Rotate(Vector3.up, horizontal * 12f);
            _rotation += vertical * 12f;
            _rotation = Mathf.Clamp(_rotation, -60f, 60f);
            cameraTransform.localEulerAngles = new Vector3(-_rotation, cameraTransform.localEulerAngles.y, 0f);
        }

        private void PlayerFiring()
        {
            if (!_weaponReady)
                return;

            _weaponReady = false;
            
            if (_bullets.Count(b => !b.gameObject.activeSelf) > 0 && _bullets.Count != 0)
            {
                bullet = _bullets.FirstOrDefault(b => !b.gameObject.activeSelf && bullet);
                if (bullet)
                {
                    bullet.gameObject.SetActive(true);
                    bullet.transform.parent = bulletParentTransform;
                }
            }
            else
            {
                bullet = Instantiate(bullet, bulletParentTransform, true);
                _bullets.Add(bullet);
            }

            if (!bullet) return;
            
            var bulletTransform = bullet.transform;
            bulletTransform.position = bulletParentTransform.position + bulletParentTransform.forward;
            bulletTransform.rotation = bulletParentTransform.rotation;
            bulletTransform.parent = null;
            bullet.name = "Bullet";
            
            bullet.RigidBody.AddForce(bulletParentTransform.transform.forward * (800 * Time.deltaTime), ForceMode.Impulse);

            StartCoroutine(DeactivateBullet(bullet.RigidBody));
        }

        private void OnCollisionEnter(Collision collision)
        {
            var currentCollider = collision.collider;

            if (currentCollider is not BoxCollider) return;
            
            gameObject.SetActive(false);
            CheckPlayerPositionAndTeleport();
        }
        
        private void CheckPlayerPositionAndTeleport()
        {
            const int attempts = 50;
            var enemies = FindObjectsOfType<Enemy>().Where(enemy => enemy.gameObject.activeSelf).ToList();
            Vector3 position;
            for (var i = 0; i != attempts; i++)
            {
                var newX = Random.Range(-3f, 3f);
                var newZ = Random.Range(-3f, 3f);
                position = new Vector3(newX, 0.2f, newZ);
                var positionIsNearAnyEnemy = enemies.Any(enemy => Vector3.Distance(position, enemy.transform.position) <= 3f);
                if (positionIsNearAnyEnemy) continue;
                transform.position = position;
                break;
            }
            gameObject.SetActive(true);
        }

        private IEnumerator DeactivateBullet(Rigidbody currentBullet)
        {
            yield return new WaitForSeconds(5f);
            currentBullet.velocity = Vector3.zero;
            currentBullet.angularVelocity = Vector3.zero;
            currentBullet.transform.parent = bulletParentTransform;
            currentBullet.gameObject.SetActive(false);
            yield return null;
        }
    }
}