using System;
using System.Collections;
using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class ArmController : MonoBehaviour
	{
		[SerializeField] bool needleHitVein;
		[SerializeField] bool needleHitArm;
		[SerializeField] bool resettingArm;
		[SerializeField, Min(0f)] float armMoveSpeed = 4f;
		[SerializeField, Min(0f)] float armResetSpeedMultiplier = 2f;
		[SerializeField] Vector3 startPosition;
		[SerializeField] BoxCollider veinCollider;
		[SerializeField] MeshCollider armCollider;
		
		Camera mainCam;
		NeedlePoint needlePoint;
		WaitForEndOfFrame waitForEndOfFrame;

		public BoxCollider VeinCollider => veinCollider;
		public MeshCollider ArmCollider => armCollider;

		void OnEnable()
		{
			startPosition = transform.position;
		}

		void OnDisable()
		{
			needlePoint.onNeedleHitArm -= OnNeedleHit;
		}
		
		void Awake()
		{
			waitForEndOfFrame = new WaitForEndOfFrame();
			mainCam = Camera.main;
			needlePoint = FindObjectOfType<NeedlePoint>();
			needlePoint.onNeedleHitArm += OnNeedleHit;
		}

		void Update()
		{
			if (resettingArm) return;
			
			if ((needleHitVein) is false)
			{
				Vector3 pos = transform.position;
				pos.y = Mathf.Lerp(pos.y, mainCam.transform.position.y, armMoveSpeed * Time.deltaTime);
				transform.position = pos;
			}
		}

		void OnNeedleHit((bool arm, bool vein) hit)
		{
			needleHitArm = hit.arm;
			needleHitVein = hit.vein;

			switch (hit)
			{
				case { arm: true, vein: false }: 
					StartCoroutine(nameof(MoveToStartPosition));
					break;
				case { arm: true or false, vein: true }:
					Debug.Log("YAY!!");
					break;
			}
		}

		IEnumerator MoveToStartPosition()
		{
			Vector3 pos = transform.position;
			resettingArm = Math.Abs(pos.y - startPosition.y) > 0.1f;

			while (resettingArm)
			{
				pos.y = Mathf.Lerp(pos.y, startPosition.y, armMoveSpeed * armResetSpeedMultiplier * Time.deltaTime);
				transform.position = pos;
				yield return waitForEndOfFrame;

				if (Math.Abs(pos.y - startPosition.y) <= 0.1f)
					resettingArm = false;
			}
			
			needleHitArm = false;
			needleHitVein = false;
			
			yield return waitForEndOfFrame;
		}
	}
}