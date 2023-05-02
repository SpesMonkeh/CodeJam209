using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace P209
{
	[DisallowMultipleComponent]
	public class SceneManager : MonoBehaviour
	{
		const int INVALID_SCENE_INDEX = -1;

		static int ActiveSceneBuildIndex
		{
			get
			{
				Scene activeScene = UnitySceneManager.GetActiveScene();
				return activeScene.IsValid() ? activeScene.buildIndex : INVALID_SCENE_INDEX;
			}
		}

		public void GoToScene(int index)
		{
			if (ActiveSceneBuildIndex == index || UnitySceneManager.GetSceneByBuildIndex(index).IsValid() is false) return;
			UnitySceneManager.LoadScene(index);
		}
	}
}