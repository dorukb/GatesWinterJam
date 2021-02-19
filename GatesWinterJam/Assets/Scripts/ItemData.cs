using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item Data")]
public class ItemData: ScriptableObject
{
    // ID is the name of the scriptable object asset.


    public Sprite hiddenSprite;
    public Sprite revealedSprite;

    // used only for display. subject to change.
    public string itemDisplayName;
    public ItemType type;
}

public enum ItemType
{
    Valuable = 0,
    Regular = 1,
    Worthless = 2
}
