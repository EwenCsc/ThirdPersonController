namespace Ewengine.ThirdPersonController
{
	using System;
	using UnityEngine;

	[Serializable]
	public class CameraHotspot
	{
		#region Enum
		public enum CameraHotspotType
		{
			Base,
			Talking,
		}
		#endregion Enum

		#region Fields
		[SerializeField] private CameraHotspotType _hotspotType = CameraHotspotType.Base;
		[SerializeField] private Vector3 _offset = Vector3.zero;
		[SerializeField][Range(0.01f, 179.0f)] private float _fov = 60.0f;
		[SerializeField][Range(0.0f, 10.0f)] private float _distanceToOrigin = 5.0f;
		#endregion Fields

		#region Properties
		public CameraHotspotType HotspotType { get => _hotspotType; }
		public Vector3 Offset { get => _offset; }
		public float Fov {get => _fov;}
		public float DistanceToOrigin { get => _distanceToOrigin;}
		#endregion Properties
	}
}