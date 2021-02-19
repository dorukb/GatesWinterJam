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
    private int index = 0;
    public float typingSpeed;


    public List<GameObject> characters;
    public GameObject dialogueNextButton;

    public List<Line> lines;
    public void SetupScene()
    {
        lines = new List<Line>();

        GameManager.CandidateSpeakers speakers = GameManager.Instance.GetSessionSpeakers();

        string debugText = "In session: " + GameManager.Instance.currentDialogueSession + " ";
        foreach(string speakerName in speakers.speakerNames)
        {
            debugText += speakerName + " ";
        }


       debugText += " can be spoken to.";
        Debug.Log(debugText);

        for(int i = 0; i < characters.Count; i++)
        {
            var dCharScript = characters[i].GetComponent<DialogueCharacter>();
            string chName = dCharScript.data.name;
            if (speakers.speakerNames.Contains(chName))
            {
                // activate highlight/indicator UI.
                dCharScript.ShowIndicator(true);
            }
            else
            {
                dCharScript.ShowIndicator(false);
            }

        }

    }
    public void ActivateScreen(CharacterData data, bool canTalk, bool useAlternativeDialog)
    {
        if (useAlternativeDialog)
        {
            lines = data.dialogues[GameManager.Instance.currentDialogueSession - 1].altLines;
        }
        else
        {
            lines = data.dialogues[GameManager.Instance.currentDialogueSession - 1].defaultLines;
        }


        charactersPanel.SetActive(false);
        toAuctionButton.SetActive(false);
        dialogueScreen.gameObject.SetActive(true);
        dialogueScreen.Setup(data);

        if (canTalk)
        {
            StartTyping();
        }
        else
        {
            dialogueNextButton.SetActive(false);
        }

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
        if (lines.Count <= 0) return;

        if(textDisplay.text == lines[index].text)
        {
            dialogueNextButton.SetActive(true);
        }
    }
    IEnumerator Type()
    {
        foreach (char letter in lines[index].text.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void ResetDialogue()
    {
        index = 0;
        textDisplay.text = "";
        StopAllCoroutines();
    }
    public void NextSentence()
    {
        dialogueNextButton.SetActive(false);
        if(index < lines.Count -1)
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
