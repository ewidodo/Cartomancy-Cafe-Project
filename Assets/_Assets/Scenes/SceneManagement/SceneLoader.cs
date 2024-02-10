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
        public AK.Wwise.Event sceneEndEvent;
    }
    [SerializeField] private List<SceneData> scenes = new();
    private Dictionary<string, SceneData> _sceneDict = new();

    private string currentScene = "Init";

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (_sceneDict.TryGetValue(currentScene, out SceneData sceneData)) sceneData.sceneEndEvent.Post(this.gameObject);
        sceneTransitionManager.FadeToBlack(fadeDuration).setOnComplete(() =>
            { 
                SceneManager.LoadScene(sceneName);
            });
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneDict.TryGetValue(scene.name, out SceneData sceneData)) sceneData.sceneStartEvent.Post(this.gameObject);
        sceneTransitionManager.FadeFromBlack(fadeDuration);
        currentScene = scene.name;
    }
}
