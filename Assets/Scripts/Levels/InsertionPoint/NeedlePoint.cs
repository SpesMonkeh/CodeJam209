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