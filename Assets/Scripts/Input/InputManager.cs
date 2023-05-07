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
		
		static PlayerControls playerControls;

		public Accelerometer Accelerometer => Accelerometer.current;
		public Vector3 Acceleration => Accelerometer.acceleration.value;
		
		public void OnPrimaryTouch(InputAction.CallbackContext context) { }
		
		public void OnGyroscope(InputAction.CallbackContext context)
		{
			Vector3 inputVector = context.ReadValue<Vector3>();

			const int zero = 0;
			const float threshold = .01f;
			set_to_zero_if_at_threshold(val: ref inputVector.x);
			set_to_zero_if_at_threshold(val: ref inputVector.y);
			set_to_zero_if_at_threshold(val: ref inputVector.z);

			angularVelocityAction?.Invoke(inputVector);

			void set_to_zero_if_at_threshold(ref float val) => val = Mathf.Abs(val) >= threshold ? val : zero;
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
			if (sensor is null || sensor.enabled) return; 
			InputSystem.EnableDevice(sensor);
		}

		public static void DisableSensor<TSensor>(TSensor sensor) where TSensor : Sensor
		{
			if (sensor is null || sensor.enabled is false) return;
			InputSystem.DisableDevice(sensor);
		}
	}
}