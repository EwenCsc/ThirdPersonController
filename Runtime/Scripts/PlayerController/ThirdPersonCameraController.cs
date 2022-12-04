namespace Ewengine.ThirdPersonController
{
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class ThirdPersonCameraController : MonoBehaviour
	{
		#region Fields
		#region Serialized Fields
		[SerializeField] private ThirdPersonCharacterPlayerController _controller = null;
		[SerializeField] private Transform _followedTransform = null;

		[SerializeField][Range(1.0f, 10.0f)] private float _sensibility = 5.0f;
		[SerializeField][Range(1.0f, 10.0f)] private float _distanceToOrigin = 5.0f;

		[SerializeField] Vector2 _pitchMinMax = new Vector2(-70.0f, 70.0f);
		[SerializeField] AnimationCurve _pitchAcceleration = new AnimationCurve();

		[SerializeField][Range(0.0f, 1.0f)] private float _rotationSmoothTime = 0.1f;
		#endregion Serialized Fields

		#region Internal Fields
		private float _yaw = 0.0f;
		private float _pitch = 0.0f;
		private Vector3 _currentRotation = Vector3.zero;
		private Vector3 _rotationSmoothDamp = Vector3.zero;
		#endregion Internal Fields
		#endregion Fields

		#region Methods
		private void Update()
		{
			InputAction lookAction = _controller.LookAction;
			Vector2 lookInput = lookAction.ReadValue<Vector2>();

			_yaw += lookInput.x * _sensibility;
			_pitch -= lookInput.y * _sensibility * _pitchAcceleration.Evaluate(Mathf.InverseLerp(_pitchMinMax.x, _pitchMinMax.y, _pitch));
			_pitch = Mathf.Clamp(_pitch, _pitchMinMax.x, _pitchMinMax.y);

			transform.position = _followedTransform.position;

			_currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationSmoothDamp, _rotationSmoothTime);
			transform.eulerAngles = _currentRotation;

			transform.position += -transform.forward * _distanceToOrigin;
		}
		#endregion Methods
	}
}