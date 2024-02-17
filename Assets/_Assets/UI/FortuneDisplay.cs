using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FortuneDisplay : Singleton<FortuneDisplay>
{
    public List<Fortune> displayedFortunes = new();

    [Header("Display References")]
    public TextMeshProUGUI fortuneName;
    public TextMeshProUGUI fortunePosition;


    public void AddFortune(Fortune fortune)
    {
        displayedFortunes.Add(fortune);
    }

    public void RemoveCurrentFortune(Fortune fortune)
    {
        if (displayedFortunes.Contains(fortune))
        {
            displayedFortunes.Remove(fortune);
        }
    }

    public void DisplayCurrentFortune()
    {
        if (displayedFortunes.Count <= 0)
        {
            // display nothing or default
            return;
        }

        // display displayedFortunes[displayedFortunes.Count - 1]
    }
}
