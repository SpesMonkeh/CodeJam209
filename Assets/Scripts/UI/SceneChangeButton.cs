using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public class SceneChangeButton : MonoBehaviour
	{
		[SerializeField, Min(0f)] protected int goToSceneIndex;
		
		protected Button sceneChangeButton;
		protected SceneManager sceneManager;
		
		protected const int ZERO = 0;
		protected const int OFF_BY_ONE_MITIGATOR = 1;

		protected static int ActiveSceneBuildIndex => SceneManager.GetActiveScene().buildIndex;
		protected static int SceneIndicesInBuild => SceneManager.sceneCountInBuildSettings - OFF_BY_ONE_MITIGATOR;
		
		void Awake()
		{
			sceneChangeButton = GetComponent<Button>();
			sceneChangeButton.onClick?.AddListener(GoToScene);
		}

		void OnDisable()
		{
			sceneChangeButton.onClick?.RemoveListener(GoToScene);
		}

		protected void GoToScene()
		{
			if (goToSceneIndex > SceneIndicesInBuild)
			{
				Debug.LogError($"Unable to load scene with build index {goToSceneIndex.ToString()}");
				return;
			}
			if (goToSceneIndex == ActiveSceneBuildIndex)
			{
				Debug.LogWarning($"Scene with index {goToSceneIndex.ToString()} is already the active scene.");
				return;
			}
			SceneManager.LoadScene(goToSceneIndex);
		}
	}
}