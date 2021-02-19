using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScreenUI : MonoBehaviour
{
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI bioText;
    public Image speakerIcon;


    public CharacterData currentData;
    public void Setup(CharacterData data)
    {
        currentData = data;

        speakerName.text = data.displayName;
        bioText.text = data.bioText;
        speakerIcon.sprite = data.icon;

    }
}
