using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public Recipe recipe;
    public CraftingSystem craftingSystem;

    public Transform IngredientsImagesParent;
    public Image productImage;
    public GameObject IngredientImagePrefab;

    public void Initialize(Recipe _recipe, CraftingSystem _craftingSystem)
    {
        recipe = _recipe;
        craftingSystem = _craftingSystem;

        productImage.sprite = recipe.product.image;

        foreach(Item ingredient in recipe.ingredients)
        {
            GameObject image = Instantiate(IngredientImagePrefab, IngredientsImagesParent);
            image.GetComponent<Image>().sprite = ingredient.image;
        }
    }

    public void Craft()
    {
        craftingSystem.Craft(this);
    }
}
