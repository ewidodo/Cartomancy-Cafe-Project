using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AK.Wwise.Event> musicEvents = new();
    private Dictionary<string, AK.Wwise.Event> musicEventsDict = new();
    private AK.Wwise.Event currentMusic;

    private void Start()
    {
        // Initialize dictionary
        foreach (AK.Wwise.Event audioEvent in musicEvents)
        {
            musicEventsDict.Add(audioEvent.Name, audioEvent);
        }
    }

    public void PostEvent(string eventName)
    {
        musicEventsDict.TryGetValue(eventName, out AK.Wwise.Event audioEvent);
        if (audioEvent == null) return;

        audioEvent.Post(this.gameObject);
    }

    //Wwise works so differently than FMOD that all the functions I originally had in this script can just be handled in Wwise itself LMAO
}
