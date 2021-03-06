using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    public List<Item> items;
    public int gold = 1000;

    public void BuyItem(Item item)
    {
        if(item.price <= gold)
        {
            gold -= item.price;
            items.Add(item);
        }
    }

    public void SellItem(Item item)
    {
        if(items.Contains(item))
        {
            items.Remove(item);
            gold += item.price;
        }
    }
}
