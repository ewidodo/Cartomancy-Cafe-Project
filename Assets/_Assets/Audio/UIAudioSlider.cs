using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAudioSlider : MonoBehaviour
{
    [Tooltip("\"bus:/[vcaName]\"")]
    private string rtpcName;
    public AK.Wwise.RTPC gameParameter;
    // RTPCValue_type.RTPCValue_Global
    // Why must I hardcode it like this Wwise :(
    private int io_rValueType = 1;

    [SerializeField] private string displayNameOverride = "";
    [SerializeField] private GameObject nameObj;

    [SerializeField] private GameObject valueObj;

    public void Start()
    {
        //bus = FMODUnity.RuntimeManager.GetBus(busPath);
        rtpcName = gameParameter.Name;
        nameObj.GetComponent<TMP_Text>().text = ParseDisplayName();
        InitVolume();
    }

    // Save settings to PlayerPrefs on destroy.
    private void OnDestroy()
    {
        float volume = GetComponent<Slider>().value;
        PlayerPrefs.SetFloat(rtpcName, volume);
        //Debug.Log("Saved " + rtpcName + " Slider: " + volume);
    }

    // Display name will be the same as the FMOD Bus, unless displayNameOverride exists.
    private string ParseDisplayName()
    {
        if (displayNameOverride != "")
        {
            return displayNameOverride;
        }

        return gameParameter.Name;
    }

    // Load saved volume, or default to bus volume if none is saved.
    private void InitVolume()
    {
        float volume;

        if (PlayerPrefs.HasKey(rtpcName))
        {
            volume = PlayerPrefs.GetFloat(rtpcName);
            AkSoundEngine.SetRTPCValue(rtpcName, TransformVolume(volume));
        }
        else
        {
            AkSoundEngine.GetRTPCValue(rtpcName, this.gameObject, 0, out volume, ref io_rValueType);
        }

        //Debug.Log("Loading " + rtpcName + " Slider: " + volume);
        GetComponent<Slider>().value = volume;
    }

    // Sliders should be 0% - 200% volume (default 100%)
    public void UpdateVolume(System.Single volume)
    {
        AkSoundEngine.SetRTPCValue(rtpcName, TransformVolume(volume));
        //Debug.Log("SetVolume: " + TransformVolume(volume));
    }

    public void UpdateText(System.Single value)
    {
        // Convert from range 0 - 2 to range 0 - 200
        valueObj.GetComponent<TMP_Text>().text = (value * 100).ToString("f0") + "%";
    }

    private float TransformVolume(System.Single volume)
    {
        // Slider Range: 0 - 2f
        // Game Parameter Range: 0 - 100f
        return volume * 100 / 2; 
    }
}
