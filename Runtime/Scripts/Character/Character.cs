namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;

	public class Character : MonoBehaviour
	{
		#region Fields
		#region Serialize Fields
		[Header("Components")]
		[SerializeField] private Rigidbody _rigidbody = null;

		[Header("Movement parameters")]
		[SerializeField][Range(0.0f, 20.0f)] private float _moveSpeed = 10.0f;
		[SerializeField][Range(0.0f, 1.0f)] private float _turnSmoothTime = 0.1f;

		[Header("Jump parameters")]
		[SerializeField] private float _jumpForce = 2.0f;
		[SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
		#endregion Serialize Fields

		#region Internal Fields
		private float _turnSmoothVelocity = 0.0f;

		private bool _isJumping = false;
		#endregion Internal Fields
		#endregion Fields

		#region Properties
		public bool IsJumping { get => _isJumping; }
		public Rigidbody Rigidbody { get => _rigidbody; }
		#endregion Properties

		#region Methods
		#region Publics
		public bool IsGrounded()
		{
			bool isGrounded = Physics.CheckSphere(transform.position, 0.1f, LayerMask.GetMask("Ground"));
			_isJumping = isGrounded == false;
			return isGrounded;
		}

		public void RequestJump()
		{
			if (IsGrounded() == false)
			{
				return;
			}

			_isJumping = true;
			_rigidbody.AddForceAtPosition(Vector3.up * _jumpForce, transform.position, _forceMode);
		}

		public void ComputeMovement(Vector2 moveInput, float YRoration)
		{
			Vector3 direction = (transform.forward * moveInput.y) + (transform.right * moveInput.x).normalized;

			ComputeRotation(direction, YRoration);

			float velocityY = _rigidbody.velocity.y;
			Vector3 velocity = direction * _moveSpeed;
			velocity.y = velocityY;

			_rigidbody.velocity = velocity;
		}
		#endregion Publics

		#region Internals
		private void ComputeRotation(Vector3 direction, float yRotation)
		{
			if (direction != Vector3.zero)
			{
				float targetRotation = yRotation;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, _turnSmoothTime);
			}
		}
		#endregion Internals
		#endregion Methods
	}
}