namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MenuNavigation
    {
        public static class MenuScenes
        {
            public const string Game = "MainGame";
            public const string Main = "MainMenu";
            public const string Settings = "SettingsMenu";
        }

        public void StartGame()
        {
            SceneManager.LoadScene(MenuScenes.Game);
        }

        public void NavigateToMainMenu()
        {
            Debug.Log("load main menu");
            SceneManager.LoadScene(MenuScenes.Main);
        }

        public void NavigateToSettingsMenu()
        {
            Debug.Log("load settings");
            SceneManager.LoadScene(MenuScenes.Settings);
        }
    }
}