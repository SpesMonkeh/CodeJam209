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
		static InputManager instance;

		const int ZERO = 0;
		const float POINT_O_ONE = 0.01f;

		public static InputManager Instance
		{
			get
			{
				if (instance != null) return instance;

				InputManager[] inputManagers = FindObjectsOfType<InputManager>();
				if (inputManagers.Length <= ZERO) return instance; // TODO Håndtér tilfælde, hvor inputManagers.Length == ZERO

				instance = inputManagers[ZERO];
				for (int i = 1; i < inputManagers.Length; i++)
				{
					if (instance == inputManagers[i]) continue;
					Debug.LogWarning($"P2 :::: Destroying unnecessary InputManager => {inputManagers[i]}");
					Destroy(inputManagers[i]);
				}
				return instance;
			}
		}

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
			if (sensor is not null) InputSystem.EnableDevice(sensor);
		}

		static void DisableSensor<TSensor>(TSensor sensor) where TSensor : Sensor
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