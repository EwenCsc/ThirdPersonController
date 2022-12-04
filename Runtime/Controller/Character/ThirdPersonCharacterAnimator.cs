namespace Ewengine.ThirdPersonController
{
	using UnityEngine;

	public class ThirdPersonCharacterAnimator : MonoBehaviour
	{
		#region Fields
		[SerializeField] private ThirdPersonCharacterController _characterController = null;

		[SerializeField] private Animator _animator = null;

		[SerializeField] private string _forwardVelocityParameter = string.Empty;
		[SerializeField] private string _rightdVelocityParameter = string.Empty;
		[SerializeField] private string _isJumpingParameter = string.Empty;
		[SerializeField] private string _isGroundedParameter = string.Empty;
		#endregion Fields

		#region Methods
		private void LateUpdate()
		{
			Transform characterTransform = _characterController.transform;
			Vector3 velocity = _characterController.Rigidbody.velocity.normalized;

			_animator.SetFloat(_forwardVelocityParameter, Vector3.Dot(characterTransform.forward, velocity));
			_animator.SetFloat(_rightdVelocityParameter, Vector3.Dot(characterTransform.right, velocity));

			if (_characterController.IsJumping)
			{
				_animator.SetTrigger(_isJumpingParameter);
			}

			_animator.SetBool(_isGroundedParameter, _characterController.IsGrounded());
		}
		#endregion Methods
	}
}
