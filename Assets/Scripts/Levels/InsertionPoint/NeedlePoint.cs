using System;
using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(BoxCollider))]
	public sealed class NeedlePoint : MonoBehaviour
	{
		public Action<(bool hitArm, bool hitVein)> onNeedleHitArm = delegate {  };

		BoxCollider pointCollider;

		void Awake()
		{
			pointCollider = GetComponent<BoxCollider>();
		}

		/*void OnCollisionEnter(Collision other)
		{
			GameObject otherGameObject = other.gameObject;

			Debug.Log($"Hit {otherGameObject}!");
			
			if (otherGameObject.TryGetComponent(out ArmPart armPart) is false || armPart.IsMainVeinPart is false) return;

			Debug.Log("Hit ArmPart!");

			bool needleHitVein = pointCollider.bounds.Intersects(armPart.ArmController.VeinCollider.bounds);
			Debug.Log("Hit VEIN!");

			onNeedleHitVein?.Invoke(needleHitVein);
		}*/

		void OnTriggerEnter(Collider otherCollider)
		{
			GameObject otherGameObject = otherCollider.gameObject;
		
			(bool hitArm, bool hitVein) hitPoints = (false, false);

			hitPoints.hitArm = otherGameObject.TryGetComponent(out ArmController armController);

			if (otherGameObject.TryGetComponent(out ArmPart armPart) && armPart.IsMainVeinPart)
			{
				if (armPart.IsMainVeinPart)
					hitPoints.hitVein = pointCollider.bounds.Intersects(armPart.ArmController.VeinCollider.bounds);
				else
					hitPoints.hitArm = true;
			}
			
			if (hitPoints is (false, false)) return;
			
			onNeedleHitArm?.Invoke(hitPoints);
		}
	}
}