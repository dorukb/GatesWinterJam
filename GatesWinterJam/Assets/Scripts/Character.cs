using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string name;
    public int budget;
    public float turnDuration = 3f; // for AI players
    // desired items
    // not interested items
    [HideInInspector] public int playerIndex;

    private AuctionManager auctionManager;
    private void Start()
    {
        auctionManager = FindObjectOfType<AuctionManager>();
    }
    public void PlayTurn(int playerIndex) // returns amount offered for this turn.
    {
        // start timer here to automatically end if no decision is made in 'turnDuration' seconds.
        

        this.playerIndex = playerIndex;
        if (playerIndex == 0) // human player, wait for input on the UI
        {
            StartCoroutine(TurnTimer());
        }
        else // AI player, do AI logic to decide, then call MadeDecision.
        {
            Invoke("MakeAIDecision", turnDuration);
        }
    }
    private void MakeAIDecision()
    {
        int offer = AuctionManager.maxOffer + 50;
        auctionManager.MadeDecision(playerIndex, offer);
    }
    private IEnumerator TurnTimer()
    {
        float startTime = Time.time;
        while(Time.time - startTime < turnDuration)
        {
            //update UI to reflect remaining turn time.
            yield return new WaitForSecondsRealtime(1f);
        }

        if(playerIndex == 0) //human
        {
            MadeOffer(0); // pass when the time runs out.
        }
        else // ai
        {
            // run AI decision code here, send the decision to auction manager.
            // currently, just increase the max offer by 50.

            
        }
        Debug.Log("Player " + playerIndex + " passed this turn.");
    }
    public void MadeOffer(int amount) // called by submit button on Offer UI
    {
        StopAllCoroutines(); // stop timer once the decision is made. needed for human players mostly.
        Debug.Log("stopped timer.");
        auctionManager.MadeDecision(playerIndex, amount);
    }
}
