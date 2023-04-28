using UnityEngine;
using UnityEngine.UI;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public sealed class MainMenuButton : MonoBehaviour
	{
		Button mainMenuButton;

		void Awake()
		{
			mainMenuButton = GetComponent<Button>();
			mainMenuButton.onClick.AddListener(SceneManager.GoToMainMenuScene);
		}

		void OnDisable() => mainMenuButton.onClick.RemoveListener(SceneManager.GoToMainMenuScene);
	}
}