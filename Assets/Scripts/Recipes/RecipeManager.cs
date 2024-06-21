using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;
    public void Awake() { Instance = this; }

    [SerializeField] private List<RecipeToUnlock> recipesToUnlock;

    private void Start()
    {
        for (int i = 0; i < recipesToUnlock.Count; i++)
        {
            recipesToUnlock[i].Initialize();
        }
    }

    public void UnlockAllRecipes()
    {
        ClearRecipes();

        for (int i = 0; i < GameManager.Instance.recipeList.Count; i++)
        {
            UnlockRecipe(GameManager.Instance.recipeList[i]);
        }
    }

    public void UnlockRecipe(Recipe recipe)
    {
        if (recipe.recipeType == AdditionalPanelType.Crafting) CraftingSystem.Instance.recipes.Add(recipe);
        if (recipe.recipeType == AdditionalPanelType.Furance) FuranceSystem.Instance.recipes.Add(recipe);
        if (recipe.recipeType == AdditionalPanelType.Anvil) AnvilSystem.Instance.recipes.Add(recipe);

        for (int i = 0; i < recipesToUnlock.Count; i++)
        {
            if (recipesToUnlock[i].recipe == recipe)
            {
                recipesToUnlock[i].SwtichState(true);
                return;
            }
        }
    }

    public void ClearRecipes()
    {
        CraftingSystem.Instance.recipes.Clear();
        FuranceSystem.Instance.recipes.Clear();
        AnvilSystem.Instance.recipes.Clear();
    }
}
