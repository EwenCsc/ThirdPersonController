namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class PlayerInputsHandler : IDisposable
	{
		#region Constants
		private const string MOVE_ACTION_KEY = "Move";
		private const string LOOK_ACTION_KEY = "Look";
		private const string INTERACT_ACTION_KEY = "Interact";
		private const string JUMP_ACTION_KEY = "Jump";
		#endregion Constants

		#region Fields
		private bool _inputsEnabled = true;

		private InputActionMap _inputActionMap = null;

		private InputAction _moveAction = null;
		private InputAction _lookAction = null;
		private InputAction _interactAction = null;
		private InputAction _jumpAction = null;
		#endregion Fields

		#region Properties
		public Vector2 MoveInputValue
		{
			get
			{
				if (_moveAction == null || _inputsEnabled == false)
				{
					return Vector2.zero;
				}

				return _moveAction.ReadValue<Vector2>();
			}
		}

		public Vector2 LookInputValue
		{
			get
			{
				if (_lookAction == null || _inputsEnabled == false)
				{
					return Vector2.zero;
				}

				return _lookAction.ReadValue<Vector2>();
			}
		}
		#endregion Properties

		#region Events
		private Action _jumpedEvent = null;
		public event Action Jumped
		{
			add { _jumpedEvent -= value; _jumpedEvent += value; }
			remove { _jumpedEvent -= value; }
		}

		private Action _interactedEvent = null;
		public event Action Interacted
		{
			add { _interactedEvent -= value; _interactedEvent += value; }
			remove { _interactedEvent -= value; }
		}
		#endregion Events

		#region Constructor
		public PlayerInputsHandler(PlayerInput playerInput)
		{
			_inputActionMap = playerInput.currentActionMap;

			_moveAction = _inputActionMap.FindAction(MOVE_ACTION_KEY, true);
			_lookAction = _inputActionMap.FindAction(LOOK_ACTION_KEY, true);

			_interactAction = _inputActionMap.FindAction(INTERACT_ACTION_KEY, true);
			_interactAction.started += OnInteractRequested;

			_jumpAction = _inputActionMap.FindAction(JUMP_ACTION_KEY, true);
			_jumpAction.started += OnJumpRequested;
		}
		#endregion Constructor

		#region Methods
		public void Dispose()
		{
			_inputActionMap = null;

			_moveAction = null;
			_lookAction = null;

			if (_interactAction != null)
			{
				_interactAction.started -= OnInteractRequested;
				_interactAction = null;
			}

			if (_jumpAction != null)
			{
				_jumpAction.started -= OnJumpRequested;
				_jumpAction = null;
			}

			_jumpedEvent = null;
			_interactedEvent = null;
		}

		public void EnableInputs(bool enable)
		{
			_inputsEnabled = enable;
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				EnableInputs(_inputsEnabled == false);
			}
		}

		#region Callbacks
		private void OnJumpRequested(InputAction.CallbackContext callbackContext)
		{
			if (_inputsEnabled == false)
			{
				return;
			}

			_jumpedEvent?.Invoke();
		}

		private void OnInteractRequested(InputAction.CallbackContext obj)
		{
			if (_inputsEnabled == false)
			{
				return;
			}

			_interactedEvent?.Invoke();
		}
		#endregion Callbacks
		#endregion Methods
	}
}
