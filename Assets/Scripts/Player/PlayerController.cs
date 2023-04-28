using UnityEngine;
using UnityEngine.InputSystem;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Rigidbody))]
	public sealed class PlayerController : MonoBehaviour
	{
		[SerializeField, Min(0f)] float moveSpeed = 75f;

		[SerializeField] Vector3 angularVelocity;
		[SerializeField] Vector3 acceleration = Vector3.zero;
		[SerializeField] Vector3 desiredVelocity;
		[SerializeField] Rigidbody rigidBody;
		
		[SerializeField] Accelerometer accelerometer;

		const int ZERO = 0;

		void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
		}

		void Start()
		{
			accelerometer = InputManager.Instance.Accelerometer;
		}

		void Update()
		{
			acceleration = accelerometer.acceleration.value;
		}

		void FixedUpdate()
		{
			Vector3 currentVelocity = rigidBody.velocity;
			Vector3 convertedAccelerationAxes = ConvertAccelerationAxes();
			
			desiredVelocity = convertedAccelerationAxes * (moveSpeed * Time.fixedDeltaTime);

			if (desiredVelocity != Vector3.zero)
				rigidBody.velocity = desiredVelocity;
			else if (desiredVelocity == Vector3.zero && currentVelocity != Vector3.zero)
				rigidBody.velocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1 * Time.fixedDeltaTime);
		}

		Vector3 ConvertAccelerationAxes()
		{
			Vector3 axes = accelerometer.acceleration.value;
			return new Vector3(axes.x, ZERO, axes.y);
		}
	}
}

