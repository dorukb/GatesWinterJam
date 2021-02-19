using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharData")]
public class CharacterData : ScriptableObject
{

    // id is the name of the scriptable object asset itself.


    // used only for display purposes, not identification.
    public string displayName;

    [TextArea(3, 10)]
    public string bioText;
    public Sprite mainSprite;
    public Sprite icon;
    public int charIndex;

    public List<Dialogue> dialogues;
}
