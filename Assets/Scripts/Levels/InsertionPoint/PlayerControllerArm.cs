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

		const int ZERO_DEGREES = 0;
		const float PIVOT_ROTATION_STEP_MULTIPLIER = 6f;
		const string PIVOT = "Pivot";

		void Awake()
		{
			pivotPoint = GetPivotPointGameObject();
		}

		void Start()
		{
			SetPivotRotationToZero();
			
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
			Vector3 pivotEuler = pivotPoint.transform.rotation.eulerAngles;

			float xTargetRot = pivotEuler.x + acceleration.x;
			float zTargetRot = pivotEuler.z + acceleration.z;
			
			float xTargetClamped = Mathf.Clamp(xTargetRot, xMinAngle, xMaxAngle);
			float zTargetClamped = Mathf.Clamp(zTargetRot, zMinAngle, zMaxAngle);
			
			Vector3 targetRotation = new(xTargetClamped, ZERO_DEGREES, zTargetClamped);

			float maxDegreesDelta = Time.deltaTime * PIVOT_ROTATION_STEP_MULTIPLIER;
			
			Quaternion pivotQuaternion = Quaternion.Euler(pivotEuler);
			Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
			
			Vector3 finalRotation = Quaternion.RotateTowards(pivotQuaternion, targetQuaternion, maxDegreesDelta).eulerAngles;
			finalRotation.y = ZERO_DEGREES;
			
			pivotPoint.transform.rotation = Quaternion.Euler(finalRotation);
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
		
		GameObject GetPivotPointGameObject() => 
			(from childTransform 
					in GetComponentsInChildren<Transform>() 
				where childTransform.gameObject.name
					is PIVOT
				select childTransform.gameObject)
			.FirstOrDefault();
	}
}