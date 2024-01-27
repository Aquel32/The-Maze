using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuranceSlot : MonoBehaviour
{
    public Recipe recipe;
    public FuranceSystem furanceSystem;

    public Transform IngredientsImagesParent;
    public Image productImage;
    public GameObject IngredientImagePrefab;

    public void Initialize(Recipe _recipe, FuranceSystem _furanceSystem)
    {
        recipe = _recipe;
        furanceSystem = _furanceSystem;

        productImage.sprite = recipe.product.image;

        foreach(Item ingredient in recipe.ingredients)
        {
            GameObject image = Instantiate(IngredientImagePrefab, IngredientsImagesParent);
            image.GetComponent<Image>().sprite = ingredient.image;
        }
    }

    public void Craft()
    {
        furanceSystem.Craft(this);
    }
}
