using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public AK.Wwise.Event buttonPressSound;

    public void PlayButtonSound()
    {
        buttonPressSound.Post(this.gameObject);
    }

    public void UpdateRecipeTutorial()
    {
        if (TutorialManager.Instance != null) TutorialManager.Instance.readRecipeBook = true;
    }

    public void UpdateTutorialStatus()
    {
        if (TutorialManager.Instance != null) TutorialManager.Instance.seenTutorial = true;
    }  
    
    public void LoadTutorial()
    {
        if (TutorialManager.Instance != null && !TutorialManager.Instance.seenTutorial) SceneLoader.Instance.LoadScene("Tutorial");
        else SceneLoader.Instance.LoadScene("Gameplay");
    }    

    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void LoadNextDay()
    {
        SceneLoader.Instance.LoadNextDay();
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
