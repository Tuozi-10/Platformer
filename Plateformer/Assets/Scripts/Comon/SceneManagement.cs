using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private int scene;
    [SerializeField] private bool asyncLoad = false;

    public void LoadScene()
    {
        if (asyncLoad)
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    public void LoadScene(int index)
    {
        scene = index;
        LoadScene();
    }

    public void Quit()
    {
        if (Application.isEditor)
        {
            Debug.LogError("Application Quit");
        }
        else
        {
            Application.Quit();
        }
    }
}
