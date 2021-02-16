using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfferUI : MonoBehaviour
{
    public GameObject OfferUIVisual;
    public TextMeshProUGUI offerAmount;
    public int increaseAmount = 50;

    public int roundStartOffer;
    public int currentOffer = 0;

    public void ShowOfferUI(int roundStartOffer)
    {
        OfferUIVisual.SetActive(true);
        this.roundStartOffer = roundStartOffer;
        currentOffer = roundStartOffer;
        offerAmount.text = currentOffer.ToString();
    }
    public void HideOfferUI()
    {
        OfferUIVisual.SetActive(false);
    }
    public void IncreaseOfferButton()
    {
        currentOffer += increaseAmount;
        offerAmount.text = currentOffer.ToString();
    }
    public void DecreaseOfferButton()
    {
        currentOffer -= increaseAmount;
        if (currentOffer < roundStartOffer) currentOffer += increaseAmount;

        offerAmount.text = currentOffer.ToString();
    }

    public void SubmitOfferButton()
    {
        FindObjectOfType<AuctionManager>().players[0].GetComponent<Character>().MadeOffer(currentOffer);
    }

    public void PassButton()
    {
        FindObjectOfType<AuctionManager>().players[0].GetComponent<Character>().MadeOffer(roundStartOffer);
    }
}
