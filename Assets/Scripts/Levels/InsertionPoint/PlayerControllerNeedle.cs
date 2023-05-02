using UnityEngine;
using UnityEngine.InputSystem;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class PlayerControllerNeedle : MonoBehaviour
	{
		[SerializeField, Min(0f)] float gyroSensitivity = 100f;
		[SerializeField] Vector3 acceleration = Vector3.zero;
		[SerializeField] Accelerometer accelerometer;

		const int ZERO = 0;

		void Start()
		{
			accelerometer = GameManager.Instance.InputManager.Accelerometer;
		}
		
		void Update()
		{
			acceleration = accelerometer.acceleration.ReadValue();
			
			Quaternion tfRotation = transform.rotation;
			Vector3 eulerRotation = tfRotation.eulerAngles;
			Vector3 accelerometerRotation = ConvertAccelerationAxes() * (gyroSensitivity * Time.deltaTime);

			AdjustConstrainedEulerRotation(accelerometerRotation.x, ref eulerRotation.x);
			AdjustConstrainedEulerRotation(accelerometerRotation.z, ref eulerRotation.z);
			
			tfRotation.eulerAngles = eulerRotation;
			transform.rotation = tfRotation;
		}
		
		Vector3 ConvertAccelerationAxes() => new(acceleration.x, ZERO, acceleration.z);

		static void AdjustConstrainedEulerRotation(float accelerometerAxis, ref float eulerRotationAxis)
		{
			const float constraint = 5f;
			if (eulerRotationAxis is < -constraint or > constraint) return;
			eulerRotationAxis += accelerometerAxis;
		}
	}
}