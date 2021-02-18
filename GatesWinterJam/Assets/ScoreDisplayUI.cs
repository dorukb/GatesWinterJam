using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;

    private void Start()
    {
        scoreDisplay.text = GameManager.Instance.GetPlayerScore().ToString();
    }
    public void UpdateScore(int score)
    {
        scoreDisplay.text = score.ToString();
    }
}
