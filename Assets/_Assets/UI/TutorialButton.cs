using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (TutorialManager.Instance == null) gameObject.SetActive(false);
        if (!TutorialManager.Instance.seenTutorial) gameObject.SetActive(false);
    }
}
