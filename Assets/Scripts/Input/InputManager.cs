using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class InputManager : MonoBehaviour, PlayerControls.IGeneralActions
	{
		public static Action<Vector2> primaryTouchAction = delegate {  };
		public static Action<Vector3> angularVelocityAction = delegate {  };
		
		Gyroscope gyroscope;

		static PlayerControls playerControls;

		const int ZERO = 0;
		const float POINT_O_ONE = 0.01f;

		public Accelerometer Accelerometer => Accelerometer.current;
		public Vector3 Acceleration => Accelerometer.acceleration.value;
		
		public void OnPrimaryTouch(InputAction.CallbackContext context) { }
		
		public void OnGyroscope(InputAction.CallbackContext context)
		{
			Vector3 inputVector = context.ReadValue<Vector3>();
			
			CheckIfSmallAmount(ref inputVector.x);
			CheckIfSmallAmount(ref inputVector.y);
			CheckIfSmallAmount(ref inputVector.z);

			angularVelocityAction?.Invoke(inputVector);
		}

		void OnEnable()
		{
			playerControls ??= new PlayerControls();
			playerControls.Enable();
			playerControls.General.SetCallbacks(this);
		}

		void OnDisable()
		{
			playerControls.Disable();
			
			DisableSensor(Gyroscope.current);
			DisableSensor(Accelerometer);
		}

		void Awake()
		{
			EnableSensor(Gyroscope.current);
			EnableSensor(Accelerometer);
		}

		static void EnableSensor<TSensor>(TSensor sensor) where TSensor : Sensor
		{
			if (sensor is not null) 
				InputSystem.EnableDevice(sensor);
		}

		public static void DisableSensor<TSensor>(TSensor sensor) where TSensor : Sensor
		{
			if (sensor is null || sensor.enabled is false) return;
			Debug.Log($"P2 :::: Disabling sensor {sensor.name}");
			InputSystem.DisableDevice(sensor);
		}

		static void CheckIfSmallAmount(ref float value)
		{
			if (Mathf.Abs(value) >= POINT_O_ONE) return;
			value = ZERO;
		}
	}
}