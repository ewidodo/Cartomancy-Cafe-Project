using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonDontDestroy<ScoreManager>
{
    public int score;

    public void ResetScore()
    {
        score = 0;
    }
}
