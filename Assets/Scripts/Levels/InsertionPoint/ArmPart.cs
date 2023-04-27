using System;
using UnityEngine;

namespace P209
{
	public sealed class ArmPart : MonoBehaviour
	{
		[SerializeField] bool isMainVeinPart;

		ArmController armController;
		
		public bool IsMainVeinPart => isMainVeinPart;
		public ArmController ArmController => armController;

		const string MAIN_VEIN_NAME = "Main Vein";
		
		void OnEnable()
		{
			isMainVeinPart = gameObject.name is MAIN_VEIN_NAME;
		}

		void Awake()
		{
			armController = GetComponentInParent<ArmController>();
		}
	}
}