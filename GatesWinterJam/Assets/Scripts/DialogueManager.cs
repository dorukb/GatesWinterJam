using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject charactersPanel;
    public DialogueScreenUI dialogueScreen;
    public GameObject toAuctionButton;

    public Image speakerIcon;
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI speakerName;

    private int index = 0;
    public float typingSpeed;


    public List<GameObject> characters;
    public GameObject dialogueNextButton;

    public List<Line> lines;
    private CharacterData currentCharData;
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
        currentCharData = data;
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
            // configure dialogue here, also play SFX if needed.
            if (lines[index].speakerIcon == null)
            {
                speakerIcon.gameObject.SetActive(false);
            }
            else
            {
                speakerIcon.gameObject.SetActive(true);
            }
            speakerIcon.sprite = lines[0].speakerIcon;
            speakerName.text = lines[0].speakerName;
            if (lines[0].hasSoundEffect)
            {
                // play sound effect
                AudioManager.Instance.PlaySFX(lines[0].soundEffect);
            }
            StartTyping();
        }
        else
        {
            Debug.Log("disable next button for: " + data.name);
            dialogueNextButton.SetActive(false);
        }

    }
    public void TurnBackCharScreen()
    {
        ResetDialogue();

        charactersPanel.SetActive(true);
        toAuctionButton.SetActive(true);
        dialogueScreen.gameObject.SetActive(false);
    }


    public void StartTyping()
    {
        textDisplay.text = "";
        dialogueNextButton.SetActive(false);
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
 
    public void NextSentence()
    {
        dialogueNextButton.SetActive(false);
        if(index < lines.Count -1)
        {
            index++;
            textDisplay.text = "";

            // configure dialogue here, also play SFX if needed.
            if(lines[index].speakerIcon == null)
            {
                speakerIcon.gameObject.SetActive(false);
            }
            else
            {
                speakerIcon.gameObject.SetActive(true);
            }
            speakerIcon.sprite = lines[index].speakerIcon;
            speakerName.text = lines[index].speakerName;
            if (lines[index].hasSoundEffect)
            {
                // play sound effect
                AudioManager.Instance.PlaySFX(lines[index].soundEffect);
            }
            StartCoroutine(Type());

        }
        else
        {
            textDisplay.text = "";
            index = 0;
            TurnBackCharScreen();
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
}
