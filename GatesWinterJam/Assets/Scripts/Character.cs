using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{

    // decreased by the maxOfferAmount, if that player has won an auction, by the auction manager.
    public TextMeshProUGUI playersMoneyUI;
    public TextMeshProUGUI offerTextUI;
    public string charName;
    //public int budget;  // set in the GameManager in Start Scene.

    public float turnDuration = 3f; // for AI players

    public float desiredItemBudgetPerct = 0.8f;
    public float neutralItemBudgetPerct = 0.2f;

    // desired items
    public List<Item> desiredItems;
    // not interested items

    public int playerIndex;

    private AuctionManager auctionManager;
    private void Start()
    {
        auctionManager = FindObjectOfType<AuctionManager>();
        offerTextUI.text = "";
        UpdateMoneyDisplay();
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
    public void ResetOfferUI()
    {
        offerTextUI.text = "";
    }
    private void MakeAIDecision()
    {
        int minOffer = auctionManager.maxOffer + auctionManager.offerIncreaseAmount;
        string currItem = auctionManager.selectedItem.GetComponent<Item>().itemName;
        float currMoney = GameManager.Instance.GetCurrentMoney(playerIndex); // chars money at the start of this round.

        if (minOffer > currMoney)
        {
            // not enough money to increase the offer the minimum required amount. must pass
            ShowOfferInfo(0);
            auctionManager.MadeDecision(playerIndex, 0);
            Debug.Log("Not enough money to offer.");
        }
        else if (desiredItems.Find(i => i.itemName == currItem) != null)
        {
            // try to win this one.
            if (auctionManager.isLastRound()) 
            {
                //go all in, each round increase by 2 times the usual amount.
                int idealOffer = minOffer + auctionManager.offerIncreaseAmount;
                if (idealOffer <= currMoney)
                {
                    ShowOfferInfo(idealOffer);
                    offerTextUI.text = idealOffer.ToString();
                    auctionManager.MadeDecision(playerIndex, idealOffer);

                }
                else
                {
                    // couldnt increase by 2 times, so use all your money to at least offer minimum amount.
                    ShowOfferInfo(minOffer);
                    offerTextUI.text = minOffer.ToString();
                    auctionManager.MadeDecision(playerIndex, minOffer);
                }
            }
            else
            {
                Debug.Log("desired item, offer %80 tops. allowed budget: " + currMoney * desiredItemBudgetPerct);

                // spend max %80 of your current money.
                int idealOffer = minOffer + auctionManager.offerIncreaseAmount;
                if (idealOffer <= currMoney * desiredItemBudgetPerct) // cant spend more than %80 of current budget.
                {
                    ShowOfferInfo(idealOffer);
                    auctionManager.MadeDecision(playerIndex, idealOffer);
                }
                else if (minOffer <= currMoney * desiredItemBudgetPerct)
                {
                    // if min offer is in our budget, do that    
                    ShowOfferInfo(minOffer);
                    auctionManager.MadeDecision(playerIndex, minOffer);
                }
                else //pass
                {
                    ShowOfferInfo(0);
                    auctionManager.MadeDecision(playerIndex, 0);
                }
            }
        }
        else  // neutral item.
        {
            Debug.Log("neutral item, offer %20 tops. allowed budget: " + currMoney * neutralItemBudgetPerct);

            int idealOffer = minOffer;

            if (idealOffer <= currMoney * neutralItemBudgetPerct) // cant spend more than %20 of current budget on this.
            {
                ShowOfferInfo(idealOffer);
                auctionManager.MadeDecision(playerIndex, idealOffer);
            }
            else //pass
            {
                ShowOfferInfo(0);
                auctionManager.MadeDecision(playerIndex, 0);
            }
        }

    }
    private void ShowOfferInfo(int amount)
    {
        if(amount == 0)
        {
            offerTextUI.text = "Passed.";
        }
        else
        {
            offerTextUI.text = amount.ToString();
        }

    }
    private IEnumerator TurnTimer()
    {
        float startTime = Time.time;
        while (Time.time - startTime < turnDuration)
        {
            //update UI to reflect remaining turn time.
            yield return new WaitForSecondsRealtime(1f);
        }
        
        MadeOffer(0); // pass when the time runs out.
        Debug.Log("Player " + playerIndex + " passed this turn.");
    }
    public void MadeOffer(int amount) // called by submit button on Offer UI
    {
        StopAllCoroutines(); // stop timer once the decision is made. needed for human players.
        ShowOfferInfo(amount);
        auctionManager.MadeDecision(playerIndex, amount);
    }

    public void UpdateMoneyDisplay()
    {
        if(playersMoneyUI) playersMoneyUI.text = GameManager.Instance.GetCurrentMoney(playerIndex).ToString();
    }
}
