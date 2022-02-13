using UnityEngine;

public class MenuManager : MonoBehaviour
{
     
    public static MenuManager instance;
    
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
    }

    public void ClickOnPlay()
    {
        GameManager.LoadScene(GameManager.ScenesName.Level1);
    }
    
    
}
