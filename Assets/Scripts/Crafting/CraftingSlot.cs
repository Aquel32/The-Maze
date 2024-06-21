using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public Recipe recipe;

    public Transform IngredientsImagesParent;
    public Image productImage;
    public IngredientSlot IngredientImagePrefab;

    public void Initialize(Recipe _recipe)
    {
        recipe = _recipe;

        productImage.sprite = recipe.product.image;

        foreach(Item ingredient in recipe.ingredients)
        {
            IngredientSlot image = Instantiate(IngredientImagePrefab, IngredientsImagesParent);
            image.GetComponent<Image>().sprite = ingredient.image;
            image.item = ingredient;
            image.craftingSlot = this;
        }
    }

    public void Craft()
    {
        CraftingSystem.Instance.Craft(this);
    }
    
    public void ShowProductHint()
    {
        InventoryManager.Instance.ShowHint(recipe.product.itemName);
    }

    public void ShowIngredientHint(Item item) 
    {
        InventoryManager.Instance.ShowHint(item.itemName);
    }
}
