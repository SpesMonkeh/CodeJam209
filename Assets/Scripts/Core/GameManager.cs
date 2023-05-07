using UnityEngine;
using UnityEngine.InputSystem;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class GameManager : MonoBehaviour
	{
		InputManager inputManager;
		
		static GameManager instance;

		const string GAME_MANAGER_PREFAB_PATH = "GAME MANAGER";

		public InputManager InputManager => inputManager ??= GetElseAddComponent<InputManager>(gameObject);
		
		public static GameManager Instance
		{
			get
			{
				if (instance != null) return instance;
				GameObject prefab = Resources.Load<GameObject>(GAME_MANAGER_PREFAB_PATH);
				GameObject scenePrefab = Instantiate(prefab);
				
				instance = GetElseAddComponent<GameManager>(scenePrefab);
				DontDestroyOnLoad(instance.transform.root.gameObject);
				return instance;
			}
		}

		public Accelerometer GetAccelerometer() => InputManager.Accelerometer;
		
		static TMonoBehaviour GetElseAddComponent<TMonoBehaviour>(GameObject gameObject) where TMonoBehaviour : MonoBehaviour
			=> gameObject.GetComponentInChildren<TMonoBehaviour>() 
			   ?? gameObject.AddComponent<TMonoBehaviour>();
	}
}