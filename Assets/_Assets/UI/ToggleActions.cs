using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActions : MonoBehaviour
{
    public void Awake()
    {
        if (PlayerPrefs.GetInt("Fullscreen") == 1)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (PlayerPrefs.GetInt("Fullscreen") == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
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
