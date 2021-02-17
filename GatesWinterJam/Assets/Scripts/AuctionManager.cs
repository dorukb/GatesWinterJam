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
    int turnsPlayedThisRound;
    public void Start()
    {
        offers = new int[players.Count];
        PlaySession();
    }
    public void PlaySession()
    {
        PickItem();
        roundNumber = 0;
        currentPlayer = 0; // TODO: last sessions winner starts.
        turnsPlayedThisRound = 0;
        //StartCoroutine(PlayRound());
        PlayTurn(currentPlayer);
    }
    private void NextRound()
    {
        roundNumber++;
        if (roundNumber == maxRoundCount) EndSession();
        else
        {
            //StartCoroutine(PlayRound());
            currentPlayer = 0;
            turnsPlayedThisRound = 0;
            PlayTurn(currentPlayer);
        }
    }
    //private IEnumerator PlayRound()
    //{
    //    //    for(int i = 0; i < players.Count; i++)
    //    //    {
    //    //        currentPlayer = i;
    //    //        // show turn, activate UI if players turns.
    //    //        Debug.Log("Player " + i + "'s turn.");

    //    //        float turnTime;
    //    //        if (i == 0) //only if player
    //    //        {
    //    //            FindObjectOfType<OfferUI>().ShowOfferUI(maxOffer);
    //    //            turnTime = playerTurnTime;
    //    //        }
    //    //        else
    //    //        {
    //    //            turnTime = AITurnTime;
    //    //            FindObjectOfType<OfferUI>().HideOfferUI();
    //    //        }

    //    //        players[i].GetComponent<Character>().PlayTurn(i);
    //    //        //RegisterOffer(i, offerAmount);
    //    //        yield return new WaitForSecondsRealtime(turnTime);
    //    // refresh UI to show changes.
    ////}
    //    //NextRound();
    //}
    private void PlayTurn(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + "'s turn.");
        if (playerIndex == 0) //only if human player
        {
            FindObjectOfType<OfferUI>().ShowOfferUI(maxOffer);
        }
        else
        {
            FindObjectOfType<OfferUI>().HideOfferUI();
        }

        players[playerIndex].GetComponent<Character>().PlayTurn(playerIndex);
        //RegisterOffer(i, offerAmount);
    }
    public void MadeDecision(int playerIndex, int offerAmount) // players should call this when decision is made.
    {
        if (offerAmount != maxOffer)
        {
            Debug.Log("Player " + playerIndex + " offered " + offerAmount + " dollars");
            RegisterOffer(playerIndex, offerAmount);
        }
        else Debug.Log("Player " + playerIndex + " passed.");
        NextTurn();
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

        if (amount == 0) { Debug.Log("Now player " + playerIndex + " is winning."); } // passes
        else
        {
            offers[playerIndex] = amount;
        }
    }
    private void EndSession()
    {
        // show who won this item.
        Debug.Log("Player " + maxOfferOwner + " has won the " + selectedItem.name);

        // switch to dialogue scene
    }
    private void PickItem()
    {
        selectedItem = items[Random.Range(0, items.Count)];
        Debug.Log("selling item " + selectedItem.name + " this session.");
    }
}
