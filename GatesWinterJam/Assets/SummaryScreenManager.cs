using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummaryScreenManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI itemCounter;

    public void Start()
    {
        int valuableitemCount = GameManager.Instance.playerValuableItemCount;
        itemCounter.text = valuableitemCount + "/" + 6;
    }
}
