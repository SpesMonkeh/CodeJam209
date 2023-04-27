using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class PlayerControllerNeedle : MonoBehaviour
	{
		[SerializeField] NeedlePoint needlePoint;

		void Awake()
		{
			needlePoint = GetComponentInChildren<NeedlePoint>();
		}
	}
}