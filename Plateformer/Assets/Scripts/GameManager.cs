using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        // avoid mistakes when reloading scene
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
        InitializeGame();
    }

    private void InitializeGame()
    {
        LoadScene(ScenesName.Menu);
    }
    
    #region scene Management

    public enum ScenesName
    {
        Menu,
        Level1
    }

    private static string GetSceneName(ScenesName sceneName)
    {
        return sceneName switch
        {
            ScenesName.Menu => "Menu",
            ScenesName.Level1 => "Level1",
            _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
        };
    }
    
    public static void LoadScene(ScenesName sceneName)
    {
        SceneManager.LoadScene(GetSceneName(sceneName));
    }
    
    #endregion
}
