using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private SceneTransitionManager sceneTransitionManager;
    public float fadeDuration;


    // Start is called before the first frame update
    void Start()
    {
        LoadScene("Test");
    }

    void StartScene()
    {
        sceneTransitionManager.FadeFromBlack(fadeDuration);
    }

    void EndScene()
    {
        sceneTransitionManager.FadeToBlack(fadeDuration);
    }

    void LoadScene(string sceneName)
    {
        sceneTransitionManager.FadeToBlack(fadeDuration).setOnComplete(() =>
            { 
                SceneManager.LoadScene(sceneName); 
                sceneTransitionManager.FadeFromBlack(fadeDuration); 
            });
    }
}
