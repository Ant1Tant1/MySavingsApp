using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public Scenes scene;

    public void LoadScene()
    {
        // load scene
        SceneManager.LoadScene((int)scene);
    }

    public void LoadScene(Scenes myScene)
    {
        // load scene
        SceneManager.LoadScene((int) myScene);
    }

    public static void LoadSceneStatic(Scenes myScene)
    {
        // load scene
        SceneManager.LoadScene((int)myScene);
    }
}
