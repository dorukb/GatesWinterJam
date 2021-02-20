using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI speakerName;

    private int index = 0;
    public float typingSpeed;

    public GameObject dialogueNextButton;

    public List<Line> lines;
    public void SetupScene()
    {
        lines = new List<Line>();

        GameManager.CandidateSpeakers speakers = GameManager.Instance.GetSessionSpeakers();

        string debugText = "In session: " + GameManager.Instance.currentDialogueSession + " ";
        foreach (string speakerName in speakers.speakerNames)
        {
            debugText += speakerName + " ";
        }


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

        if (textDisplay.text == lines[index].text)
        {
            dialogueNextButton.SetActive(true);
        }
    }

    public void NextSentence()
    {
        dialogueNextButton.SetActive(false);
        if (index < lines.Count - 1)
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
