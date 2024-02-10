using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private SceneTransitionManager sceneTransitionManager;
    public float fadeDuration;

    [Serializable]
    struct SceneData
    {
        public string sceneName;
        public AK.Wwise.Event sceneStartEvent;
    }
    [SerializeField] private List<SceneData> scenes = new();
    private Dictionary<string, SceneData> _sceneDict = new();


    // Start is called before the first frame update
    void Start()
    {
        InitSceneDictionary();
        //StartScene();
        LoadScene("MainMenu");
    }

    void InitSceneDictionary()
    {
        foreach (SceneData sceneData in scenes)
        {
            _sceneDict[sceneData.sceneName] = sceneData;
        }
    }

    void StartScene()
    {
        sceneTransitionManager.FadeFromBlack(fadeDuration);
    }

    void EndScene()
    {
        sceneTransitionManager.FadeToBlack(fadeDuration);
    }

    public void LoadScene(string sceneName)
    {
        sceneTransitionManager.FadeToBlack(fadeDuration).setOnComplete(() =>
            { 
                SceneManager.LoadScene(sceneName);
                SceneManager.sceneLoaded += OnSceneLoaded;
                
                
            });
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneDict.TryGetValue(scene.name, out SceneData sceneData)) sceneData.sceneStartEvent.Post(this.gameObject);
        sceneTransitionManager.FadeFromBlack(fadeDuration);
    }
}
