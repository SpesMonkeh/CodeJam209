using System;
using UnityEngine;
using UnityEngine.UI;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public sealed class QuitGameButton : MonoBehaviour
	{
		Button quitGameButton;

		void Awake()
		{
			quitGameButton = GetComponent<Button>();
			quitGameButton.onClick?.AddListener(RequestQuitApplication);
		}
		
		void OnDisable()
		{
			quitGameButton.onClick?.RemoveListener(RequestQuitApplication);
		}

		static void RequestQuitApplication() => GameManager.Instance.QuitApplication();
	}
}