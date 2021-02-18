using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharData")]
public class CharacterData : ScriptableObject
{
    public string charName;


    [TextArea(3, 10)]
    public string bioText;
    public Sprite mainSprite;
    public Sprite icon;
    public int charIndex;

    public List<Dialogue> dialogues;
}
