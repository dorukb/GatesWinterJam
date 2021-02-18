using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject charactersPanel;
    public DialogueScreenUI dialogueScreen;
    public GameObject toAuctionButton;


    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;


    public List<GameObject> characters;
    public GameObject dialogueNextButton;

    public void SetupScene()
    {
        GameManager.CandidateSpeakers speakers = GameManager.Instance.GetSessionSpeakers();

        Debug.Log("In session: " + GameManager.Instance.currentSession + " ");
        foreach(string speakerName in speakers.speakerNames)
        {
            Debug.Log(speakerName + " ");
        }

        Debug.Log(" can be spoken to.");
    }
    public void ActivateScreen(CharacterData data)
    {

        charactersPanel.SetActive(false);
        toAuctionButton.SetActive(false);
        dialogueScreen.gameObject.SetActive(true);
        dialogueScreen.Setup(data);

        StartTyping();
    }
    public void TurnBackCharScreen(string ScreenToChange)
    {
        ResetDialogue();

        charactersPanel.SetActive(true);
        toAuctionButton.SetActive(true);
        dialogueScreen.gameObject.SetActive(false);
    }


    public void StartTyping()
    {
        textDisplay.text = "";
        StartCoroutine(Type());
    }
    void Update()
    {
        if(textDisplay.text == sentences[index])
        {
            dialogueNextButton.SetActive(true);
        }
    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void ResetDialogue()
    {
        index = 0;
        textDisplay.text = "";
    }
    public void NextSentence()
    {
        dialogueNextButton.SetActive(false);
        if(index < sentences.Length -1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());

        }
        else
        {
            textDisplay.text = "";
            index = 0;
        }
    }
}
