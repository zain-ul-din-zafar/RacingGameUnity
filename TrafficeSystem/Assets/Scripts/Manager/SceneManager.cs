using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager {
  
    // Name of All Scene
    public enum Scene{
        MainScene,
        LevelSelection,
        ShowRoom,
    }

    public static void LoadScene(Scene scene) => SceneManager.LoadScene(scene.ToString());
} 