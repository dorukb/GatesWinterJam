using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public SpriteRenderer display;
    ItemData data;
    public void SetupItem(ItemData data)
    {
        this.data = data;
        display.sprite = data.hiddenSprite;
    }
}
