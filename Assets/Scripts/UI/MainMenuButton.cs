using UnityEngine;
using UnityEngine.UI;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public sealed class MainMenuButton : MonoBehaviour
	{
		Button mainMenuButton;
		SceneManager sceneManager;

		const int MAIN_MENU_SCENE_INDEX = 0;

		void Awake()
		{
			mainMenuButton = GetComponent<Button>();
			mainMenuButton.onClick?.AddListener(GoToMainMenuScene);
		}

		void OnDisable() => mainMenuButton.onClick?.RemoveListener(GoToMainMenuScene);

		static void GoToMainMenuScene() => GameManager.Instance.SceneManager.GoToScene(MAIN_MENU_SCENE_INDEX);
	}
}