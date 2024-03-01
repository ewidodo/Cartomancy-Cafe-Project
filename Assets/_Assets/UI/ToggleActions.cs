using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActions : MonoBehaviour
{
    public void Awake()
    {
        if (PlayerPrefs.GetInt("Fullscreen") == 1)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            GetComponent<Toggle>().isOn = true;
        }
        else if (PlayerPrefs.GetInt("Fullscreen") == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void ToggleFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
        
    }
}
