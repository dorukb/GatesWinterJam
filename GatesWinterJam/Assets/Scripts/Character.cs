using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string name;
    public int budget;

    // desired items
    // not interested items
    [HideInInspector] public int playerIndex;
    public int PlayTurn(int playerIndex) // returns amount offered for this turn.
    {
        // return 0 for passing.
        this.playerIndex = playerIndex;
        //
        return 0;
    }

    public void MadeOffer(int amount)
    {
        FindObjectOfType<AuctionManager>().MadeDecision(playerIndex, amount);
    }
}
