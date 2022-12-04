namespace Ewengine.ThirdPersonController
{
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class ThirdPersonCharacterPlayerController : MonoBehaviour
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
		[SerializeField] private Character _character = null;
		[SerializeField] private ThirdPersonCameraController _camera = null;
		[SerializeField] private PlayerInput _playerInput = null;
		#endregion Serialize Fields

		#region Internal Fields
		private InputActionMap _inputActionMap = null;

		private InputAction _moveAction = null;
		private InputAction _lookAction = null;
		private InputAction _interactAction = null;
		private InputAction _jumpAction = null;
		#endregion Internal Fields
		#endregion Fields

		#region Properties
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

			_jumpAction.started += OnJumpRequested;
		}

		private void OnDisable()
		{
			_inputActionMap = null;

			_moveAction = null;
			_lookAction = null;
			_interactAction = null;

			if (_jumpAction != null)
			{
				_jumpAction.started -= OnJumpRequested;
				_jumpAction = null;
			}
		}

		private void Update()
		{
			_character.ComputeMovement(_moveAction.ReadValue<Vector2>(), _camera.transform.eulerAngles.y);
		}
		#endregion MonoBehaviour

		#region Callbacks
		private void OnJumpRequested(InputAction.CallbackContext callbackContext)
		{
			_character.RequestJump();
		}
		#endregion Callbacks
		#endregion Methods
	}
}