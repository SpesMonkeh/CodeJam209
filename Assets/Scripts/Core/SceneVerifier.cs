using System;
using UnityEngine;

namespace P209
{
	[DisallowMultipleComponent]
	public sealed class SceneVerifier : MonoBehaviour
	{
		void Awake()
		{
			var gameManager = FindObjectOfType<GameManager>();
			if (gameManager == null)
				gameManager = GameManager.Instance;
			Destroy(gameObject);
		}
	}
}