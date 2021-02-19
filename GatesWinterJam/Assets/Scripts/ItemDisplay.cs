using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public List<Item> items;
    
    public void ShowItemWithID(string id)
    {
        foreach (var it in items) it.gameObject.SetActive(false);

        Item item = items.Find(t => t.data.name == id);
        if(item != null)
        {
            item.gameObject.SetActive(true);
        }
    }
}
