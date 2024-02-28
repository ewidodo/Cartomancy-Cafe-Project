using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonDontDestroy<ScoreManager>
{
    public int score;
    public List<int> customerScores;

    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(int score)
    {
        this.score += score;
        customerScores.Add(score);
    }

    public List<int> ReturnScoreList()
    {
        return customerScores;
    }

    public void ClearScoreList()
    {
        customerScores.Clear();
    }
}
