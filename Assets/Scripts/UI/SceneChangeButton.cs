using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public sealed class SceneChangeButton : MonoBehaviour
	{
		[SerializeField, Min(0f)] int goToSceneIndex = -1;
		[SerializeField][TextArea(1, 1)] string goToSceneName = "";
		
		Button sceneChangeButton;

#if UNITY_EDITOR
		void OnValidate()
		{
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (goToSceneIndex < 0)
				goToSceneName = "SCENE INDEX CANNOT BE LESS THAN 0";
			else if (UnitySceneManager.GetSceneByBuildIndex(goToSceneIndex).IsValid())
				goToSceneName = UnitySceneManager.GetSceneByBuildIndex(goToSceneIndex).name;
			else
				goToSceneName = "Invalid Scene Index";
		}
#endif
		
		void Awake()
		{
			sceneChangeButton = GetComponent<Button>();
			sceneChangeButton.onClick.AddListener(RequestSceneChange);
		}

		void RequestSceneChange()
		{
			if (UnitySceneManager.GetSceneByBuildIndex(goToSceneIndex).IsValid() is false) return;
			SceneManager.GoToScene(goToSceneIndex);
		}
	}
}