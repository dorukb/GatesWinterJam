using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item Data")]
public class ItemData: ScriptableObject
{
    public Sprite hiddenSprite;
    public Sprite revealedSprite;
    public string itemName;
    public ItemType type;
}

public enum ItemType
{
    Valuable = 0,
    Regular = 1,
    Worthless = 2
}
