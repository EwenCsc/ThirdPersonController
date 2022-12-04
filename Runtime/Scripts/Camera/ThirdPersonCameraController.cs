namespace Ewengine.ThirdPersonController
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class ThirdPersonCameraController : MonoBehaviour
	{
		#region Fields
		#region Serialized Fields
		[Header("Component")]
		[SerializeField] private ThirdPersonCharacterPlayerController _controller = null;
		[SerializeField] private Camera _camera = null;
		[SerializeField] private Transform _origin = null;

		[Header("HotSpots")]
		[SerializeField] private List<CameraHotspot> _hotspots = null;

		[Header("Control Settings")]
		[SerializeField][Range(1.0f, 10.0f)] private float _sensibility = 5.0f;
		[SerializeField] Vector2 _pitchMinMax = new Vector2(-70.0f, 70.0f);
		[SerializeField][Range(0.0f, 1.0f)] private float _rotationSmoothTime = 0.1f;
		#endregion Serialized Fields

		#region Internal Fields
		private int _currentHotspotsIndex = 0;
		private CameraHotspot _currentHotspot = default;

		private float _yaw = 0.0f;
		private float _pitch = 0.0f;
		private Vector3 _currentRotation = Vector3.zero;
		private Vector3 _rotationSmoothDamp = Vector3.zero;

		private bool _locked = false;
		#endregion Internal Fields
		#endregion Fields

		#region Methods
		private void OnEnable()
		{
			_currentHotspot = _hotspots[_currentHotspotsIndex];
		}

		private void OnDisable()
		{
			_currentHotspot = null;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				_locked = _locked == false;
			}

			if (Input.GetKeyDown(KeyCode.A))
			{
				_currentHotspotsIndex = Mathf.Abs((_currentHotspotsIndex - 1)) % _hotspots.Count;
				_currentHotspot = _hotspots[_currentHotspotsIndex];
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				_currentHotspotsIndex = Mathf.Abs((_currentHotspotsIndex + 1)) % _hotspots.Count;
				_currentHotspot = _hotspots[_currentHotspotsIndex];
			}

			ComputeCamera();
		}

		private void ComputeCamera()
		{
			InputAction lookAction = _controller.LookAction;

			Vector2 lookInput = _locked ? Vector2.zero : lookAction.ReadValue<Vector2>();

			_yaw += lookInput.x * _sensibility;
			_pitch = Mathf.Clamp(_pitch - lookInput.y * _sensibility, _pitchMinMax.x, _pitchMinMax.y);

			transform.position = _origin.position;
			_camera.fieldOfView = _currentHotspot.Fov;

			_currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationSmoothDamp, _rotationSmoothTime);
			transform.eulerAngles = _currentRotation;

			transform.position = _origin.position + _currentHotspot.Offset - transform.forward * _currentHotspot.DistanceToOrigin;
		}
		#endregion Methods
	}
}