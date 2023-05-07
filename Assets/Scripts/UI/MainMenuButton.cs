namespace P209
{
	public sealed class MainMenuButton : SceneChangeButton
	{
		const int MAIN_MENU_SCENE_INDEX = 0;

		void OnEnable() => goToSceneIndex = MAIN_MENU_SCENE_INDEX;
	}
}