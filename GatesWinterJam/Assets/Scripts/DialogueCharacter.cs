using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCharacter : MonoBehaviour
{
    public GameObject dialogueIndicator;
    public TextMeshProUGUI nameDisplay;
    public CharacterData data;

 
    bool showDialogue = true;

    private void Start()
    {
        nameDisplay.text = data.displayName;
    }
    public void SetupDialogue()
    {
        // do item/gameplay checks here for this character to decide which dialogue it will use.
    }

    public void ShowIndicator(bool show)
    {
        dialogueIndicator.SetActive(show);
        showDialogue = show;

        //Debug.Log("Show: " + showDialogue + " for " + data.name);
    }
    public void DialogueRequested()
    {
        // character indices:
        // human player: 0
        // wolf: 1
        // knight: 2
        // alien: 3
        // egirl: 4
        // vampire: 5
        // as defined by the AuctionManager Players array in auctionScene and corresponding inventory in GameManager.

        // show "already talked" state on the indicator.
        Color color = dialogueIndicator.GetComponent<TextMeshProUGUI>().color;
        color.a = 0.35f;
        dialogueIndicator.GetComponent<TextMeshProUGUI>().color = color;

        int currentSession = GameManager.Instance.currentDialogueSession;
        bool useAlternativeDialogue = false;

        switch (data.name) // switch on scriptable object name as ID.
        {
            case "Alien": // in session 4, branch on MC having the toiletPaper
                if (currentSession == 4 && GameManager.Instance.HasItem(0, "GoldenToiletPaper"))
                {
                    useAlternativeDialogue = true;
                    Debug.Log("USING ALTERNATIVE DIALOGUE FOR ALIEN.");
                }
                break;
            case "Egirl": // always default
                break;
            case "Knight": // in session 3 branch on whether VAMPIRE has the Holy Grail or not.
                if (currentSession == 3 && GameManager.Instance.HasItem(5, "HolyGrail"))
                {
                    useAlternativeDialogue = true;
                    Debug.Log("USING ALTERNATIVE DIALOGUE FOR KNIGHT.");
                }
                break;
            case "Vampire": // in sessions 3 and 5, branch on whether THE KNIGHT has the Holy Grail or not. SAME CONDITION.

                if (currentSession == 3 || currentSession == 5)
                {
                    if (GameManager.Instance.HasItem(2, "HolyGrail")) // knight has it?
                    {
                        useAlternativeDialogue = true;
                        Debug.Log("USING ALTERNATIVE DIALOGUE FOR VAMPIRE.");
                    }
                }
                break;
            case "Wolf": // in session 4, branch on whether Wolf has the meat or not.
                if (currentSession == 4 && GameManager.Instance.HasItem(1, "Meat"))
                {
                    useAlternativeDialogue = true;
                    Debug.Log("USING ALTERNATIVE DIALOGUE FOR WOLF.");
                }
                break;
            case "/_": break;
        }
        FindObjectOfType<DialogueManager>().ActivateScreen(data, showDialogue, useAlternativeDialogue);
    }

}
