using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YelpReview : MonoBehaviour
{
    [Header("Display References")]
    public TextMeshProUGUI username;
    public TextMeshProUGUI review;
    public Image profilePicture;

    public void DisplayReview(string text)
    {
        review.text = text;
    }
}
