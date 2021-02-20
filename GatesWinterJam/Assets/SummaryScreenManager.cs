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
        itemCounter.text = valuableitemCount + "/6";
       
        if(valuableitemCount > 5) //5,6
        {
            // excellent
            summaryText.text = "Arzuladigin ölümsüz hayata kavuştun.";
        }
        else if (valuableitemCount > 2)// 3,4
        {
            //good
            summaryText.text = "Ölümsüzlüge ulaştın ancak zamanın sonuna kadar iblisin hizmetkarı olarak.";

        }
        else
        {
            summaryText.text = "En azından son günlerini eglenerek geçirdin. Tekrar dene.";

        }
    }
}
