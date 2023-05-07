using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class PlayerControllerArm : MonoBehaviour
	{
		[SerializeField, Range(0f, 10f)] float accelerationSensitivity = 10f;
		[SerializeField] Accelerometer accelerometer;
		[SerializeField] float xStartAngle;
		[SerializeField] float zStartAngle;
		[SerializeField] float xMinAngle = -10f;
		[SerializeField] float xMaxAngle = 10f;
		[SerializeField] float zMinAngle = -10f;
		[SerializeField] float zMaxAngle = 10f;
		[SerializeField] float xMinCalibratedAngle;
		[SerializeField] float xMaxCalibratedAngle;
		[SerializeField] float zMinCalibratedAngle;
		[SerializeField] float zMaxCalibratedAngle;
		[SerializeField] GameObject pivotPoint;

		const int ZERO_DEGREES = 0;
		const string PIVOT = "Pivot";

		void Awake()
		{
			pivotPoint = GetPivotPointGameObject();
		}

		void Start()
		{
			SetPivotRotationToZero();
			CalibrateAngles();
			
#if UNITY_ANDROID
			SetAccelerometer();
#endif
		}

		void Update()
		{
#if UNITY_ANDROID
			ControlRotation();
#endif
		}
		
#if UNITY_ANDROID
		void SetAccelerometer() => accelerometer = GameManager.Instance.InputManager.Accelerometer;
		
		void ControlRotation()
		{
			if (accelerometer is null) return;
			
			Vector3 acceleration = accelerometer.acceleration.ReadValue();
			LimitRotation(acceleration);
		}

		void LimitRotation(Vector3 acceleration)
		{
			Debug.Log($"P209 :::: Acceleration => {acceleration}");

			float xAccelerationMultiplied = acceleration.x * accelerationSensitivity;
			float zAccelerationMultiplied = acceleration.z * accelerationSensitivity;

			Debug.Log($"P209 :::: xAccelerationMultiplied => {xAccelerationMultiplied} :::: zAccelerationMultiplied => {zAccelerationMultiplied}");
			
			Vector3 pivotEuler = pivotPoint.transform.rotation.eulerAngles;
			Vector3 targetRotation = new(pivotEuler.x + xAccelerationMultiplied, pivotEuler.y, pivotEuler.z + zAccelerationMultiplied);
			
			Debug.Log($"P209 :::: Rotation pre-lerp => {pivotEuler}");
			Debug.Log($"P209 :::: TargetRot pre-clamp => {targetRotation}");

			targetRotation.x = Mathf.Clamp(targetRotation.x, xMinCalibratedAngle, xMaxCalibratedAngle);
			targetRotation.z = Mathf.Clamp(targetRotation.z, zMinCalibratedAngle, zMaxCalibratedAngle);

			Debug.Log($"P209 :::: TargetRot post-clamp => {targetRotation}");

			pivotEuler.x = Mathf.Lerp(pivotEuler.x, targetRotation.x, Time.deltaTime);
			pivotEuler.z = Mathf.Lerp(pivotEuler.z, targetRotation.z, Time.deltaTime);
			
			Debug.Log($"P209 :::: Rotation post-lerp => {pivotEuler}");
			
			pivotPoint.transform.rotation = Quaternion.Euler(pivotEuler);
		}
#endif
		
		void SetPivotRotationToZero()
		{
			Quaternion pivotQuaternion = pivotPoint.transform.rotation;
			Vector3 pivotEuler = pivotQuaternion.eulerAngles;

			xStartAngle = pivotEuler.x = ZERO_DEGREES;
			zStartAngle = pivotEuler.z = ZERO_DEGREES;

			pivotQuaternion = Quaternion.Euler(pivotEuler);
			pivotPoint.transform.rotation = pivotQuaternion;
		}
		
		void CalibrateAngles()
		{
			xMinCalibratedAngle = xStartAngle + xMinAngle;
			xMaxCalibratedAngle = xStartAngle + xMaxAngle;
			
			zMinCalibratedAngle = zStartAngle + zMinAngle;
			zMaxCalibratedAngle = zStartAngle + zMaxAngle;
		}
		
		GameObject GetPivotPointGameObject() => 
			(from childTransform 
					in GetComponentsInChildren<Transform>() 
				where childTransform.gameObject.name
					is PIVOT
				select childTransform.gameObject)
			.FirstOrDefault();
	}
}