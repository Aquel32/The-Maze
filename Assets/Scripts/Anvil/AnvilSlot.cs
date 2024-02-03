using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilSlot : MonoBehaviour
{
    public Recipe recipe;
    public AnvilSystem anvilSystem;

    public Transform IngredientsImagesParent;
    public Image productImage;
    public IngredientSlot IngredientImagePrefab;

    public void Initialize(Recipe _recipe, AnvilSystem _anvilSystem)
    {
        recipe = _recipe;
        anvilSystem = _anvilSystem;

        productImage.sprite = recipe.product.image;

        foreach(Item ingredient in recipe.ingredients)
        {
            IngredientSlot image = Instantiate(IngredientImagePrefab, IngredientsImagesParent);
            image.GetComponent<Image>().sprite = ingredient.image;
            image.item = ingredient;
            image.anvilSlot = this;
        }
    }

    public void Craft()
    {
        anvilSystem.Craft(this);
    }

    public void ShowProductHint()
    {
        anvilSystem.inventoryManager.ShowHint(recipe.product.itemName);
    }

    public void ShowIngredientHint(Item item)
    {
        anvilSystem.inventoryManager.ShowHint(item.itemName);
    }
}
