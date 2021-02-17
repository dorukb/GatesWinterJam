using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    public string itemName;
    public SpriteRenderer visual;

    private void Start()
    {
        visual.sprite = sprite;
    }
}
