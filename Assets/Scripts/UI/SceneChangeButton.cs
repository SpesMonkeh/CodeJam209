using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace P209
{
	[DisallowMultipleComponent][RequireComponent(typeof(Button))]
	public sealed class SceneChangeButton : MonoBehaviour
	{
		[SerializeField, Min(0f)] int goToSceneIndex = -1;
		
		Button sceneChangeButton;
		SceneManager sceneManager;

		void Awake()
		{
			sceneManager = GameManager.Instance.SceneManager;
			sceneChangeButton = GetComponent<Button>();
			sceneChangeButton.onClick.AddListener(RequestSceneChange);
		}

		void RequestSceneChange()
		{
			if (UnitySceneManager.GetSceneByBuildIndex(goToSceneIndex).IsValid() is false) return;
			sceneManager.GoToScene(goToSceneIndex);
		}
	}
}