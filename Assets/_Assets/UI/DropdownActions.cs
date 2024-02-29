using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownActions : MonoBehaviour
{
    public string ID;

    public void Awake()
    {
        GetComponent<TMP_Dropdown>().value = (PlayerPrefs.GetInt(ID) / 15) - 1;
    }

    public void SaveSetting(int setting)
    {
        switch(setting)
        {
            case 0:
                {
                    PlayerPrefs.SetInt(ID, 15);
                    break;
                }
            case 1:
                {
                    PlayerPrefs.SetInt(ID, 30);
                    break;
                }
            case 2:
                {
                    PlayerPrefs.SetInt(ID, 45);
                    break;
                }
            default:
                {
                    PlayerPrefs.SetInt(ID, 30);
                    break;
                }
        }

        
    }
}
