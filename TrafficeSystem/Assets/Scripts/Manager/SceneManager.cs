using UnityEngine;

public static class SceneLoaderManager  {
   public enum SceneName {
    GameplayScene
   }
   
   public static void LoadScene (SceneName sceneName) 
   => UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName.ToString());
   
}
