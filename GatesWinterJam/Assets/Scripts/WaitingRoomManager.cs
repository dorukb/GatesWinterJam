using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject charScreen;
    [SerializeField]
    private GameObject toAuctionButton;

    public List<GameObject> enemyScreens;

    public void ActivateScreen(int charIndex, CharacterData data)
    {

        charScreen.SetActive(false);
        toAuctionButton.SetActive(false);

        enemyScreens[charIndex].SetActive(true);

        FindObjectOfType<DialogueManager>().StartTyping();
    }
    public void TurnBackCharScreen(string ScreenToChange)
    {


        FindObjectOfType<DialogueManager>().ResetDialogue();

        charScreen.SetActive(true);
        toAuctionButton.SetActive(true);

        foreach(var screen in enemyScreens)
        {
            screen.SetActive(false);
        }
    }
}
