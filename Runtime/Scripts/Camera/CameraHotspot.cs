namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;

	[CreateAssetMenu(fileName = "CameraHotspot", menuName = "ScriptableObjects/Player/Camera")]
	public class CameraHotspot : ScriptableObject
	{
		#region Fields
		[SerializeField] private Vector3 _offset = Vector3.zero;
		[SerializeField][Range(0.01f, 179.0f)] private float _fov = 60.0f;
		[SerializeField][Range(0.0f, 10.0f)] private float _distanceToOrigin = 5.0f;
		#endregion Fields

		#region Properties
		public Vector3 Offset { get => _offset; }
		public float Fov {get => _fov;}
		public float DistanceToOrigin { get => _distanceToOrigin;}
		#endregion Properties
	}
}