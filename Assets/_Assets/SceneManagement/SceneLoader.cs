using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonDontDestroy<SceneLoader>
{
    [SerializeField] public SceneTransitionManager sceneTransitionManager;
    public float fadeDuration;
    public int dayNumber = 0;
    public int totalDays = 3;

    [Serializable]
    struct SceneData
    {
        public string sceneName;
        public AK.Wwise.Event sceneStartEvent;
        public AK.Wwise.Event sceneEndEvent;
    }
    [SerializeField] private List<SceneData> scenes = new();
    private Dictionary<string, SceneData> _sceneDict = new();
    [SerializeField] private AK.Wwise.Event initEvent;

    private string currentScene = "Init";

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitSceneDictionary();
        //StartScene();
        if (SceneManager.GetActiveScene().name == "Init")
        {
            initEvent.Post(this.gameObject);
            LoadScene("MainMenu");
        }
        else
        {
            StartScene();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        sceneTransitionManager.TransitionFrom(fadeDuration);
    }

    void EndScene()
    {
        sceneTransitionManager.TransitionTo(fadeDuration);
    }

    // Need to set up scenes for this to work
    public void LoadNextDay()
    {
        ++dayNumber;

        if (dayNumber > totalDays)
        {
            LoadScene("Credits");
            dayNumber = 0;
            return;
        }

        LoadScene("Gameplay");
    }

    public void LoadYelp()
    {
        LoadScene("Yelp");
    }

    public void LoadScene(string sceneName)
    {
        if (_sceneDict.TryGetValue(currentScene, out SceneData sceneData)) sceneData.sceneEndEvent.Post(this.gameObject);
        // Skip fade to black if screen is already black
        if (sceneTransitionManager.IsTransitionActive())
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        sceneTransitionManager.TransitionTo(fadeDuration).setOnComplete(() =>
            { 
                SceneManager.LoadScene(sceneName);
            });
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneDict.TryGetValue(scene.name, out SceneData sceneData)) sceneData.sceneStartEvent.Post(this.gameObject);
        sceneTransitionManager.TransitionFrom(fadeDuration);
        currentScene = scene.name;
    }
}
