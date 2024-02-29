using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : SingletonDontDestroy<PauseMenu>
{
    [Header("Display References")]
    public GameObject menu;


    new private void Awake()
    {
        base.Awake();
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape!");
        }

        if (!menu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        else if (menu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMenu()
    {
        Resume();
        SceneLoader.Instance.LoadScene("MainMenu");
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();
    }
}
