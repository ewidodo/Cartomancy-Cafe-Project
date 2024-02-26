using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YelpOverlay : MonoBehaviour
{
    [Header("Data Containers")]
    public List<string> positiveReviews;
    public List<string> neutralReviews;
    public List<string> negativeReviews;

    [Header("Display References")]
    public GameObject yelpReviewPrefab;

    public void Start()
    {
        List<int> scoreList = ScoreManager.Instance.ReturnScoreList();
        foreach (int score in scoreList)
        {
            AddReview(score);
        }
        ScoreManager.Instance.ClearScoreList();
    }

    public void AddReview(int score)
    {
        switch(score)
        {
            case 0:
                {
                    GameObject newReview = Instantiate(yelpReviewPrefab, this.transform);
                    newReview.GetComponent<YelpReview>().DisplayReview(ReturnRandomReview(negativeReviews));
                    break;
                }
            case 1:
                {
                    GameObject newReview = Instantiate(yelpReviewPrefab, this.transform);
                    newReview.GetComponent<YelpReview>().DisplayReview(ReturnRandomReview(neutralReviews));
                    break;
                }
            case 2:
                {
                    GameObject newReview = Instantiate(yelpReviewPrefab, this.transform);
                    newReview.GetComponent<YelpReview>().DisplayReview(ReturnRandomReview(positiveReviews));
                    break;
                }
            default:
                {
                    GameObject newReview = Instantiate(yelpReviewPrefab, this.transform);
                    newReview.GetComponent<YelpReview>().DisplayReview(ReturnRandomReview(neutralReviews));
                    break;
                }
        }
    }

    private string ReturnRandomReview(List<string> reviewList)
    {
        string review = reviewList[Random.Range(0, reviewList.Count)];
        reviewList.Remove(review);

        return review;
    }

}
