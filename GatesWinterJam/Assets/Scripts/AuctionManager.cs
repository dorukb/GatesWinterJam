﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuctionManager : MonoBehaviour
{
    public GameObject itemAnchor;
    public GameObject itemDisplay;

    public List<ItemData> items;

    public List<GameObject> players; //player 0 is human player.
    public GameObject itemSoldNotifUI;
    public TextMeshProUGUI itemSoldText;
    public TextMeshProUGUI roundCounter;

    public int offerIncreaseAmount = 50;
    public int maxRoundCount = 5; // item will be sold to highest bidder at the end of round 5. 
    // if only 1 player bids in any of the rounds(meaning no competition), then auction ends that round.

    [HideInInspector] private ItemData selectedItemData;
    [HideInInspector] public int[] offers;
    [HideInInspector] public int maxOffer;
    [HideInInspector] public int maxOfferOwner;

    GameObject currItem;
    int roundNumber;
    int currentPlayer;
    int turnsPlayedThisRound;
    bool[] passed;
    public void Awake()
    {
        offers = new int[players.Count];
        passed = new bool[players.Count];
        for (int i = 0; i < passed.Length; i++) passed[i] = false;
        //StartSession(0);
    }
    public void StartSession(int sessionNumber) // called by the game manager
    {
        PickItem(sessionNumber);

        itemSoldNotifUI.SetActive(false);

        roundNumber = 1;
        maxOffer = 0;
        maxOfferOwner = -1;
        currentPlayer = 0;
        turnsPlayedThisRound = 0;

        roundCounter.text = "Round " + roundNumber + "/" + maxRoundCount;
        for (int i = 0; i < passed.Length; i++) passed[i] = false; // reset passed states.

        
        PlayTurn(currentPlayer);
    }
    public AudioClip offerSFX;
    public void MadeDecision(int playerIndex, int offerAmount) // players should call this when decision is made.
    {
        if (offerAmount != 0)
        {
            Debug.Log("Player " + playerIndex + " offered " + offerAmount + " dollars");
            AudioManager.Instance.PlaySFX(offerSFX);
            RegisterOffer(playerIndex, offerAmount);
        }
        else
        {
            Debug.Log("Player " + playerIndex + " passed.");
            passed[playerIndex] = true;
        }
        NextTurn();
    }
    private void NextRound()
    {
        roundNumber++;

        int passedPlayers = 0;
        for(int i = 0; i < passed.Length; i++)
        {
            if (passed[i]) passedPlayers++;
        }

        if (roundNumber > maxRoundCount)
        {
            EndSession(); 
        }
        else if (passedPlayers >= players.Count-1)
        {
            EndSession();
        }
        else
        {
            //StartCoroutine(PlayRound());
            roundCounter.text = "Round " + roundNumber + "/" + maxRoundCount;

            currentPlayer = 0;
            turnsPlayedThisRound = 0;
            PlayTurn(currentPlayer);
        }
    }
    private void PlayTurn(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + "'s turn.");

        if (passed[playerIndex]) 
        {
            //this player already passed in a previous turn, cant play this turn.
            MadeDecision(playerIndex, 0); // pass directly here. can also include a time delay.
            return;
        }

        int passedPlayers = 0;
        for (int i = 0; i < passed.Length; i++)
        {
            if (passed[i]) passedPlayers++;
        }
        if(passedPlayers == players.Count - 1 && maxOfferOwner == playerIndex) 
        {
            // if all the other players are passed,
            // and if this one is the highest bidder so far,
            // this one wins immediately 
            Debug.Log("session ended early.");
            EndSession();
            return;
        }


        if (playerIndex == 0) //only if human player
        {
            float playersBudget = GameManager.Instance.GetCurrentMoney(playerIndex);
            FindObjectOfType<OfferUI>().Highlight(true);
            FindObjectOfType<OfferUI>().ShowOfferUI(maxOffer + offerIncreaseAmount, playersBudget, true);
        }
        else
        {
            FindObjectOfType<OfferUI>().Highlight(false);

            FindObjectOfType<OfferUI>().HideOfferUI();
        }

        players[playerIndex].GetComponent<Character>().PlayTurn(playerIndex);
    }
   

    private void NextTurn()
    {
        turnsPlayedThisRound++;
        currentPlayer = (currentPlayer + 1) % players.Count; //support wrap around.
        if(turnsPlayedThisRound >= players.Count) // all players have played this round, end of round
        {
            Debug.Log("Round " + roundNumber + " ended.");
            NextRound();
        }
        else
        {
            PlayTurn(currentPlayer);
        }
    }
    
    private void RegisterOffer(int playerIndex, int amount)
    {
        if (amount > maxOffer) 
        { 
            maxOffer = amount;
            maxOfferOwner = playerIndex;
            FindObjectOfType<MaxOfferUI>().ShowMaxOffer();
            Debug.Log("Now player " + playerIndex + " is winning.");
        } 

        if (amount == 0) { Debug.Log("player " + playerIndex + " has passed."); } // passes
        else
        {
            offers[playerIndex] = amount;
        }
    }

    public Image revealedItemImage;

    private void EndSession()
    {
        itemDisplay.SetActive(false);

        // show who won this item.
        FindObjectOfType<OfferUI>().HideOfferUI();
        itemSoldNotifUI.SetActive(true);
        revealedItemImage.sprite = selectedItemData.revealedSprite;

        string optionalSuffix = maxOfferOwner == 0 ? "n" : "";
        itemSoldText.text = players[maxOfferOwner].GetComponent<Character>().charData.displayName +" "+ selectedItemData.itemDisplayName + "'" +selectedItemData.iyelikEki + " kazandı" + optionalSuffix;
        Debug.Log("Player " + maxOfferOwner + " has won the " + selectedItemData.itemDisplayName);

        GameManager.Instance.DecreaseMoney(maxOfferOwner, maxOffer);
        GameManager.Instance.BoughtItem(maxOfferOwner, selectedItemData);

        if (maxOfferOwner == 0)
        {
            // update players money display
            players[0].GetComponent<Character>().UpdateMoneyDisplay();
        }

        //GameManager.Instance.SessionEnded(maxOfferOwner, 0); notification UI will call this instead.
        // switch to dialogue scene
    }

    public string GetCurrentItemDisplayName()
    {
        return selectedItemData.itemDisplayName;
    }
    public string GetCurrentItemID()
    {
        return selectedItemData.name;
    }

    public AudioClip itemRevealSound;
    private void PickItem(int sessionNumber)
    {
        selectedItemData = items[sessionNumber-1];
        FindObjectOfType<ItemDisplay>().ShowItemWithID(selectedItemData.name);
        AudioManager.Instance.PlaySFX(itemRevealSound);
        Debug.Log("selling item " + selectedItemData.itemDisplayName + " this session.");
    }
}
