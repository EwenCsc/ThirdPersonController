namespace Ewengine.ThirdPersonController
{
	using UnityEngine;

	public class CharacterAnimator : MonoBehaviour
	{
		#region Fields
		[SerializeField] private Character _character = null;

		[SerializeField] private Animator _animator = null;

		[SerializeField] private string _forwardVelocityParameter = string.Empty;
		[SerializeField] private string _rightdVelocityParameter = string.Empty;
		[SerializeField] private string _isJumpingParameter = string.Empty;
		[SerializeField] private string _isGroundedParameter = string.Empty;
		#endregion Fields

		#region Methods
		private void LateUpdate()
		{
			Transform characterTransform = _character.transform;
			Vector3 velocity = _character.Rigidbody.velocity.normalized;

			_animator.SetFloat(_forwardVelocityParameter, Vector3.Dot(characterTransform.forward, velocity));
			_animator.SetFloat(_rightdVelocityParameter, Vector3.Dot(characterTransform.right, velocity));

			if (_character.IsJumping)
			{
				_animator.SetTrigger(_isJumpingParameter);
			}

			_animator.SetBool(_isGroundedParameter, _character.IsGrounded());
		}
		#endregion Methods
	}
}
