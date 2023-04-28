using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace P209
{
	[DisallowMultipleComponent]
	public class SceneManager : MonoBehaviour
	{
		const int MAIN_MENU_SCENE_INDEX = 0;
		
		public static void GoToMainMenuScene() => GoToScene(MAIN_MENU_SCENE_INDEX);

		public static void GoToScene(int index)
		{
			int activeSceneIndex = UnitySceneManager.GetActiveScene().buildIndex;
			
			if (activeSceneIndex == index || UnitySceneManager.GetSceneByBuildIndex(index).IsValid() is false) return;
			UnitySceneManager.LoadScene(index);
		}
	}
}