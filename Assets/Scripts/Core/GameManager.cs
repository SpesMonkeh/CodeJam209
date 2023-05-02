using System;
using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class GameManager : MonoBehaviour
	{
		InputManager inputManager;
		SceneManager sceneManager;
		
		static GameManager instance;

		const string GAME_MANAGER_PREFAB_PATH = "GAME MANAGER";

		public InputManager InputManager => inputManager ??= GetElseAddComponent<InputManager>(gameObject);
		public SceneManager SceneManager => sceneManager ??= GetElseAddComponent<SceneManager>(gameObject);
		
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


		static TComponent GetElseAddComponent<TComponent>(GameObject gameObject) where TComponent : MonoBehaviour 
			=> gameObject.GetComponentInChildren<TComponent>() 
			   ?? gameObject.AddComponent<TComponent>();
	}
}