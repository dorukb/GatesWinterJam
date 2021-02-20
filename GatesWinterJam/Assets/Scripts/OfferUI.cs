using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfferUI : MonoBehaviour
{
    public Button makeOfferButton;
    public GameObject OfferUIVisual;
    public Image OfferUIImage;
    public Sprite highlightSprite;
    public Sprite defaultSprite;

    public TextMeshProUGUI offerAmount;
    public int increaseAmount;

    public int roundStartOffer;
    public int currentOffer = 0;


    private float budget;

    public void Start()
    {
        increaseAmount = FindObjectOfType<AuctionManager>().offerIncreaseAmount;
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            OfferUIImage.sprite = highlightSprite;
        }
        else
        {
            OfferUIImage.sprite = defaultSprite;
        }
    }
    public void ShowOfferUI(int roundStartOffer, float playerMoney, bool isPlayer)
    {
        budget = playerMoney;
        
        OfferUIVisual.SetActive(true);
        this.roundStartOffer = roundStartOffer;
        currentOffer = roundStartOffer;
        offerAmount.text = currentOffer.ToString();

        if (currentOffer > budget)
        {
            makeOfferButton.interactable = false;
        }
        else
        {
            makeOfferButton.interactable = true;
        }
    }
    public void HideOfferUI()
    {
        OfferUIVisual.SetActive(false);
    }
    public void IncreaseOfferButton()
    {
        currentOffer += increaseAmount;
        offerAmount.text = currentOffer.ToString();

        if (currentOffer > budget)
        {
            makeOfferButton.interactable = false;
        }
    }
    public void DecreaseOfferButton()
    {
        currentOffer -= increaseAmount;
        if (currentOffer < roundStartOffer) currentOffer += increaseAmount;

        offerAmount.text = currentOffer.ToString();

        if (currentOffer <= budget)
        {
            makeOfferButton.interactable = true;
        }
    }

    public void SubmitOfferButton()
    {
        FindObjectOfType<AuctionManager>().players[0].GetComponent<Character>().MadeOffer(currentOffer);
    }

    public void PassButton()
    {
        FindObjectOfType<AuctionManager>().players[0].GetComponent<Character>().MadeOffer(0);
    }
}
