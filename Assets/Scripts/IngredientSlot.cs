using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSlot : MonoBehaviour
{
    public Item item;
    public CraftingSlot craftingSlot;
    public FuranceSlot furanceSlot;
    public AnvilSlot anvilSlot;

    public void ShowHint()
    {
        if(craftingSlot != null) craftingSlot.ShowIngredientHint(item);
        if(furanceSlot != null) furanceSlot.ShowIngredientHint(item);
        if(anvilSlot != null) anvilSlot.ShowIngredientHint(item);
    }
}
