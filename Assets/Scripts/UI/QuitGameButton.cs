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
			quitGameButton.onClick?.AddListener(QuitApplication);
		}

		void QuitApplication()
		{
			quitGameButton.onClick?.RemoveListener(QuitApplication);
			Application.Quit();
		}
	}
}