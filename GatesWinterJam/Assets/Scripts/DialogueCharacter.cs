using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueCharacter : MonoBehaviour
{

    public TextMeshProUGUI nameDisplay;
    public CharacterData data;


    private void Start()
    {
        nameDisplay.text = data.charName;
    }
    public void SetupDialogue()
    {
        // do item/gameplay checks here for this character to decide which dialogue it will use.
    }

    public void DialogueRequested()
    {
        FindObjectOfType<DialogueManager>().ActivateScreen(data);
    }

}
