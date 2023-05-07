using System.Collections;
using System.Linq;
using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class ArmController : MonoBehaviour
	{
		[SerializeField] bool needleHitVein;
		[SerializeField] bool needleHitArm;
		[SerializeField] bool resettingTransform;
		[SerializeField, Min(0f)] float armMoveSpeed = 4f;
		[SerializeField, Min(0f)] float armResetSpeedMultiplier = 2f;
		[SerializeField] Vector3 startPosition;
		[SerializeField] Vector3 startRotation;
		[SerializeField] GameObject pivotPoint;
		[SerializeField] BoxCollider veinCollider;
		[SerializeField] MeshCollider armCollider;
		
		Camera mainCam;
		NeedlePoint needlePoint;
		WaitForEndOfFrame waitForEndOfFrame;

		const string PIVOT = "Pivot";

		public BoxCollider VeinCollider => veinCollider;
		public MeshCollider ArmCollider => armCollider;

		void OnEnable()
		{
			Transform tf = transform;
			startPosition = tf.localPosition;
			startRotation = tf.localRotation.eulerAngles;
		}

		void OnDisable()
		{
			needlePoint.onNeedleHitArm -= OnNeedleHit;
		}
		
		void Awake()
		{
			waitForEndOfFrame = new WaitForEndOfFrame();
			mainCam = Camera.main;
			pivotPoint = GetPivotPointGameObject();
			needlePoint = FindObjectOfType<NeedlePoint>();
			needlePoint.onNeedleHitArm += OnNeedleHit;
		}

		void Update()
		{
			if (resettingTransform || needleHitVein) return;
			
			Vector3 position = transform.position;
			position.y = Mathf.Lerp(position.y, mainCam.transform.position.y, armMoveSpeed * Time.deltaTime);
			transform.position = position;
		}

		void OnNeedleHit((bool arm, bool vein) hit)
		{
			needleHitArm = hit.arm;
			needleHitVein = hit.vein;

			switch (hit)
			{
				case { arm: true, vein: false }: 
					StartCoroutine(nameof(ResetTransform));
					break;
				case { arm: true or false, vein: true }:
					Debug.Log("YAY!!");
					break;
			}
		}

		IEnumerator ResetTransform()
		{
			const float min_position_diff = .1f;
			const float min_rotation_diff = .01f;

			Transform tf = transform;
			Vector3 position = tf.position;
			Vector3 rotation = tf.rotation.eulerAngles;

			float yPositionDiff = Mathf.Abs(position.y - startPosition.y);
			float xRotationDiff = Mathf.Abs(rotation.x - startRotation.x);
			float zRotationDiff = Mathf.Abs(rotation.z - startRotation.z);

			bool resettingPosition = yPositionDiff > min_position_diff;
			bool resettingRotation = xRotationDiff > min_rotation_diff || zRotationDiff > min_rotation_diff;
			resettingTransform = resettingPosition || resettingRotation;
			
			while (resettingTransform)
			{
				position.y = Mathf.Lerp(position.y, startPosition.y, armMoveSpeed * armResetSpeedMultiplier * Time.deltaTime);
				rotation = Vector3.Lerp(rotation, startRotation, armResetSpeedMultiplier * Time.deltaTime);
				
				transform.position = position;
				transform.rotation = Quaternion.Euler(rotation);
				
				yield return waitForEndOfFrame;

				if (resettingPosition) continue;
				
				resettingTransform = false;
				needleHitArm = needleHitVein = false;
			}
			
			yield return waitForEndOfFrame;
		}
		
		GameObject GetPivotPointGameObject() => 
			(from childTransform
					in GetComponentsInChildren<Transform>()
				where childTransform.name
					is PIVOT
				select childTransform.gameObject)
			.FirstOrDefault();
	}
}