using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Bullets;
using TouchControlsKit;
using UnityEngine;

namespace ExternalAssets.VictorsAssets.TouchControlsKit_Lite.Content.FirstPersonExample.Scripts
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private Transform bulletParentTransform;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Bullet bullet;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private int maxBulletsCount;
        
        private List<Bullet> _bullets = new();
        
        private Transform _myTransform;
        private bool _bind;
        private float _rotation;
        private bool _prevGrounded;
        private float _weaponReadyTime;
        private bool _weaponReady = true;


        private void Awake() => _myTransform = transform;

        private void Update()
        {
            HandleWeaponReady();
            HandlePlayerInput();
        }

        private void FixedUpdate() => HandlePlayerMovement();

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
                moveDirection *= 7f;

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
                if (_bullets.Count(b => b.gameObject.activeSelf) > maxBulletsCount)
                    return;

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

        private IEnumerator DeactivateBullet(Rigidbody currentBullet)
        {
            yield return new WaitForSeconds(5f);
            currentBullet.gameObject.SetActive(false);
            currentBullet.velocity = Vector3.zero;
            currentBullet.angularVelocity = Vector3.zero;
            currentBullet.transform.parent = bulletParentTransform;
            yield return null;
        }
    }
}