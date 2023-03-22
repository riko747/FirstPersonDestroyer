using TouchControlsKit;
using UnityEngine;

namespace ExternalAssets.VictorsAssets.TouchControlsKit_Lite.Content.FirstPersonExample.Scripts
{
    public class FirstPersonExample : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        
        private bool _bind;
        private Transform _myTransform;
        private CharacterController _controller;
        private float _rotation;
        private bool _jump, _prevGrounded, _isProjectileCube;
        private float _weapReadyTime;
        private bool _weapReady = true;

        
        private void Awake()
        {
            _myTransform = transform;
            _controller = GetComponent<CharacterController>();
        }
        
        private void Update()
        {
            if( _weapReady == false )
            {
                _weapReadyTime += Time.deltaTime;
                if( _weapReadyTime > .15f )
                {
                    _weapReady = true;
                    _weapReadyTime = 0f;
                }
            }

            if( TCKInput.GetAction( "fireBtn", EActionEvent.Press ) )
                PlayerFiring();

            var look = TCKInput.GetAxis( "RotationJoystick" );
            PlayerRotation( look.x, look.y );
        }

        private void FixedUpdate()
        {
            var move = TCKInput.GetAxis( "MovementJoystick" ); // NEW func since ver 1.5.5
            PlayerMovement( move.x, move.y );
        }

        
        private void PlayerMovement( float horizontal, float vertical )
        {
            var grounded = _controller.isGrounded;

            var moveDirection = _myTransform.forward * vertical;
            moveDirection += _myTransform.right * horizontal;

            moveDirection.y = -10f;

            if( _jump )
            {
                _jump = false;
                moveDirection.y = 25f;
                _isProjectileCube = !_isProjectileCube;
            }

            if( grounded )            
                moveDirection *= 7f;
            
            _controller.Move( moveDirection * Time.fixedDeltaTime );

            if( !_prevGrounded && grounded )
                moveDirection.y = 0f;

            _prevGrounded = grounded;
        }
        
        private void PlayerRotation( float horizontal, float vertical )
        {
            _myTransform.Rotate( 0f, horizontal * 12f, 0f );
            _rotation += vertical * 12f;
            _rotation = Mathf.Clamp( _rotation, -60f, 60f );
            cameraTransform.localEulerAngles = new Vector3( -_rotation, cameraTransform.localEulerAngles.y, 0f );
        }
        
        private void PlayerFiring()
        {
            if( !_weapReady )
                return;

            _weapReady = false;

            var primitive = GameObject.CreatePrimitive( _isProjectileCube ? PrimitiveType.Cube : PrimitiveType.Sphere );
            primitive.transform.position = ( _myTransform.position + _myTransform.forward );
            primitive.transform.localScale = Vector3.one * .2f;
            var rBody = primitive.AddComponent<Rigidbody>();
            var camTransform = cameraTransform;
            rBody.AddForce(camTransform.forward * Random.Range( 10f, 15f ), ForceMode.Impulse );
            Destroy( primitive, 3.5f );
        }
    }
}