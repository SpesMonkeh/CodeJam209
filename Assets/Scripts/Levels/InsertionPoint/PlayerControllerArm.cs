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
		[SerializeField] GameObject pivotPoint;

		const string PIVOT = "Pivot";

		void Awake()
		{
			pivotPoint = GetPivotPointGameObject();
		}

		void Start()
		{
			Vector3 position = transform.position;
			xStartAngle = position.x;
			zStartAngle = position.z;
			
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
			Debug.Log($"P209 ::::\nAcceleration => {acceleration}\n");

			float xAccelerationMultiplied = acceleration.x * accelerationSensitivity;
			float zAccelerationMultiplied = acceleration.z * accelerationSensitivity;

			Debug.Log($"P209 ::::\nxAccelerationMultiplied => {xAccelerationMultiplied}\nzAccelerationMultiplied => {zAccelerationMultiplied}\n");
			
			Vector3 rotationEuler = pivotPoint.transform.rotation.eulerAngles;
			Vector3 targetRotation = new(rotationEuler.x + xAccelerationMultiplied, rotationEuler.y, rotationEuler.z + zAccelerationMultiplied);
			
			Debug.Log($"P209 ::::\nRotation pre-lerp => {rotationEuler}\n");
			Debug.Log($"P209 ::::\nTargetRot pre-clamp => {targetRotation}\n");

			targetRotation.x = Mathf.Clamp(targetRotation.x, xStartAngle + xMinAngle, xStartAngle + xMaxAngle);
			targetRotation.z = Mathf.Clamp(targetRotation.z, zStartAngle + zMinAngle, zStartAngle + zMaxAngle);

			Debug.Log($"P209 ::::\nTargetRot post-clamp => {targetRotation}\n");

			rotationEuler.x = Mathf.Lerp(rotationEuler.x, targetRotation.x, Time.deltaTime);
			rotationEuler.z = Mathf.Lerp(rotationEuler.z, targetRotation.z, Time.deltaTime);
			
			Debug.Log($"P209 ::::\nRotation post-lerp => {rotationEuler}\n");
			
			pivotPoint.transform.rotation = Quaternion.Euler(rotationEuler);
		}
#endif
		
		GameObject GetPivotPointGameObject() => 
			(from childTransform 
					in GetComponentsInChildren<Transform>() 
				where childTransform.gameObject.name
					is PIVOT
				select childTransform.gameObject)
			.FirstOrDefault();
	}
}