using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionManager : MonoBehaviour
{
    public List<GameObject> items;
    public List<GameObject> players; //player 0 is human player.
    public float playerTurnTime = 30f;
    public float AITurnTime = 3f;
    public GameObject selectedItem;
    public int[] offers;
    public static int maxOffer;
    public int maxOfferOwner;

    public int maxRoundCount = 3; // item will be sold to highest bidder at the end of round 3. 
    // if only 1 player bids in any of the rounds(meaning no competition), then auction ends that round.
    int roundNumber;
    int currentPlayer;

    public void Start()
    {
        offers = new int[players.Count];
        PlaySession();
    }
    public void PlaySession()
    {
        PickItem();
        roundNumber = 1;
        StartCoroutine(PlayRound());
    }
    public void NextRound()
    {
        roundNumber++;
        if (roundNumber == maxRoundCount) EndSession();
        else
        {
            StartCoroutine(PlayRound());
        }
    }
    public IEnumerator PlayRound()
    {
        for(int i = 0; i < players.Count; i++)
        {
            currentPlayer = i;
            // show turn, activate UI if players turns.
            Debug.Log("Player " + i + "'s turn.");

            float turnTime;
            if (i == 0) //only if player
            {
                FindObjectOfType<OfferUI>().ShowOfferUI(maxOffer);
                turnTime = playerTurnTime;
            }
            else
            {
                turnTime = AITurnTime;
                FindObjectOfType<OfferUI>().HideOfferUI();
            }

            players[i].GetComponent<Character>().PlayTurn(i);
            //RegisterOffer(i, offerAmount);
            yield return new WaitForSecondsRealtime(turnTime);
            // refresh UI to show changes.
        }
        NextRound();
    }

    public void MadeDecision(int playerIndex, int offerAmount) // players should call this when decision is made.
    {
        if(playerIndex == currentPlayer)
        {
            RegisterOffer(playerIndex, offerAmount);

            Debug.Log("Offered " + offerAmount + " dollars");
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

        if (amount == 0) { Debug.Log("Now player " + playerIndex + " is winning."); } // passes
        else
        {
            offers[playerIndex] = amount;
        }
    }
    public void EndSession()
    {
        // show who won this item.
        Debug.Log("Player " + maxOfferOwner + " has won the " + selectedItem.name);

        // switch to dialogue scene
    }
    public void PickItem()
    {
        selectedItem = items[Random.Range(0, items.Count)];
        Debug.Log("selling item " + selectedItem.name + " this session.");
    }
}
