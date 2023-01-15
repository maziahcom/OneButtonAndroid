using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI tmproScore;
    private int score;
    private int multiplyer = 10;

    public void InitUI()
    {
        score = 0;
        SetScoreText();
    }
    public void ScoreAdd(int add)
    {
        score += add * multiplyer;
        SetScoreText();
    }
    private void SetScoreText()
    {
        tmproScore.text = score.ToString("000000");
    }
}
