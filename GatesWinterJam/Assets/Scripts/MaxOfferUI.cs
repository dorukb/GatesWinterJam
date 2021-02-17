using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxOfferUI : MonoBehaviour
{
    public TextMeshProUGUI maxOfferInfo;

    private int maxOffer;

    public void ShowMaxOffer()
    {
        maxOffer = FindObjectOfType<AuctionManager>().maxOffer;
        maxOfferInfo.text = maxOffer.ToString();
    }
    
}
