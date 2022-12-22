namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class ThirdPersonCharacterPlayerController : MonoBehaviour
	{
		#region Enum
		private enum State
		{
			Invalid,
			Normal,
			Talking,
		}
		#endregion Enum

		#region Fields
		[Header("Components")]
		[SerializeField] private Character _character = null;
		[SerializeField] private ThirdPersonCameraController _camera = null;
		[SerializeField] private PlayerInput _playerInput = null;

		private PlayerInputsHandler _playerInputsHandler = null;
		#endregion Fields

		#region Properties
		public PlayerInputsHandler PlayerInputsHandler { get => _playerInputsHandler; }
		#endregion Properties

		#region Methods
		#region MonoBehaviour
		private void OnDestroy()
		{
			if (_playerInputsHandler != null)
			{
				_playerInputsHandler.Jumped -= JumpRequested;
				_playerInputsHandler.Interacted -= InteractRequested;

				_playerInputsHandler.Dispose();
				_playerInputsHandler = null;
			}
		}

		private void Start()
		{
			_playerInputsHandler = new PlayerInputsHandler(_playerInput);
			_playerInputsHandler.Jumped += JumpRequested;
			_playerInputsHandler.Interacted += InteractRequested;
		}

		private void Update()
		{
			_playerInputsHandler.Update();
			_character.ComputeMovement(_playerInputsHandler.MoveInputValue, _camera.transform.eulerAngles.y);
		}
		#endregion MonoBehaviour

		#region Callbacks
		private void JumpRequested()
		{
			_character.RequestJump();
		}

		private void InteractRequested()
		{
			Debug.Log("Interact");
		}
		#endregion Callbacks
		#endregion Methods
	}
}