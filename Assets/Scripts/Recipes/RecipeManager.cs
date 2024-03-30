using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private ExperienceSystem experienceSystem;
    [SerializeField] private List<RecipeToUnlock> recipesToUnlock;

    private void Start()
    {
        experienceSystem = GetComponent<ExperienceSystem>();

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
        if (recipe.recipeType == AdditionalPanelType.Crafting) experienceSystem.craftingSystem.recipes.Add(recipe);
        if (recipe.recipeType == AdditionalPanelType.Furance) experienceSystem.furanceSystem.recipes.Add(recipe);
        if (recipe.recipeType == AdditionalPanelType.Anvil) experienceSystem.anvilSystem.recipes.Add(recipe);

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
        experienceSystem.craftingSystem.recipes.Clear();
        experienceSystem.furanceSystem.recipes.Clear();
        experienceSystem.anvilSystem.recipes.Clear();
    }
}
