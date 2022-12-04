namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class ThirdPersonCharacterController : MonoBehaviour
	{
		#region Constants
		private const string MOVE_ACTION_KEY = "Move";
		private const string LOOK_ACTION_KEY = "Look";
		private const string FIRE_ACTION_KEY = "Interact";
		private const string JUMP_ACTION_KEY = "Jump";
		#endregion Constants

		#region Fields
		#region Serialize Fields
		[Header("Components")]
		[SerializeField] private ThirdPersonCameraController _camera = null;
		[SerializeField] private Rigidbody _rigidbody = null;
		[SerializeField] private PlayerInput _playerInput = null;

		[Header("Movement parameters")]
		[SerializeField][Range(0.0f, 20.0f)] private float _moveSpeed = 10.0f;
		[SerializeField][Range(0.0f, 1.0f)] private float _turnSmoothTime = 0.1f;

		[Header("Jump parameters")]
		[SerializeField] private float _jumpForce = 2.0f;
		[SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
		#endregion Serialize Fields

		#region Internal Fields
		private InputActionMap _inputActionMap = null;

		private InputAction _moveAction = null;
		private InputAction _lookAction = null;
		private InputAction _interactAction = null;
		private InputAction _jumpAction = null;

		private float _turnSmoothVelocity = 0.0f;

		private bool _isJumping = false;
		#endregion Internal Fields
		#endregion Fields

		#region Properties
		public bool IsJumping
		{
			get
			{
				//if (_isJumping == true)
				//{
				//	_isJumping = false;
				//	return true;
				//}

				return _isJumping;
			}

		}

		public Rigidbody Rigidbody { get => _rigidbody; }

		public InputAction MoveAction { get => _moveAction; }
		public InputAction LookAction { get => _lookAction; }
		public InputAction FireAction { get => _interactAction; }
		public InputAction JumpAction { get => _jumpAction; }
		#endregion Properties

		#region Methods
		#region MonoBehaviour
		private void OnEnable()
		{
			_inputActionMap = _playerInput.currentActionMap;

			_moveAction = _inputActionMap.FindAction(MOVE_ACTION_KEY, true);
			_lookAction = _inputActionMap.FindAction(LOOK_ACTION_KEY, true);
			_interactAction = _inputActionMap.FindAction(FIRE_ACTION_KEY, true);
			_jumpAction = _inputActionMap.FindAction(JUMP_ACTION_KEY, true);

			_jumpAction.started += OnJumpStarted;
		}

		private void OnDisable()
		{
			_inputActionMap = null;

			_moveAction = null;
			_lookAction = null;
			_interactAction = null;

			if (_jumpAction != null)
			{
				_jumpAction.started -= OnJumpStarted;
				_jumpAction = null;
			}
		}

		private void Update()
		{
			//ComputeJump();
			ComputeMovement();
		}
		#endregion MonoBehaviour

		#region Publics
		public bool IsGrounded()
		{
			bool isGrounded = Physics.CheckSphere(transform.position, 0.1f, LayerMask.GetMask("Ground"));
			_isJumping = isGrounded == false;
			return isGrounded;
		}
		#endregion Publics

		#region Internals
		private void ComputeMovement()
		{
			Vector2 moveInput = _moveAction.ReadValue<Vector2>();
			Vector3 direction = (transform.forward * moveInput.y) + (transform.right * moveInput.x).normalized;

			if (direction != Vector3.zero)
			{
				float targetRotation = _camera.transform.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, _turnSmoothTime);
			}

			float velocityY = _rigidbody.velocity.y;
			Vector3 velocity = direction * _moveSpeed;
			velocity.y = velocityY;

			_rigidbody.velocity = velocity;
		}

		private void OnJumpStarted(InputAction.CallbackContext inputAction)
		{
			if (IsGrounded() == false)
			{
				return;
			}

			_isJumping = true;
			_rigidbody.AddForceAtPosition(Vector3.up * _jumpForce, transform.position, _forceMode);
		}
		#endregion Internals
		#endregion Methods
	}
}