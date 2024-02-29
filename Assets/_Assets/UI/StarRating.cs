using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarRating : MonoBehaviour
{
    private TextMeshProUGUI text;

    public string filledStar;
    public string emptyStar;

    [Header("Display References")]
    public TextMeshProUGUI writtenText;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (ScoreManager.Instance == null) return;

        int score = ScoreManager.Instance.score;
        if (score >= 18)
        {
            text.text = filledStar + filledStar + filledStar + filledStar + filledStar;
            writtenText.text = "5 Stars!";
        }
        else if (score >= 15)
        {
            text.text = filledStar + filledStar + filledStar + filledStar + emptyStar;
            writtenText.text = "4 Stars!";
        }
        else if (score >= 10)
        {
            text.text = filledStar + filledStar + filledStar + emptyStar + emptyStar;
            writtenText.text = "3 Stars.";
        }
        else if (score >= 5)
        {
            text.text = filledStar + filledStar + emptyStar + emptyStar + emptyStar;
            writtenText.text = "2 Stars.";
        }
        else if (score >= 0)
        {
            text.text = filledStar + emptyStar + emptyStar + emptyStar + emptyStar;
            writtenText.text = "1 Star...";
        }

        ScoreManager.Instance.ResetScore();
    }

}
