using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour, ITable
{
    public static CraftingSystem Instance;
    public void Awake() { Instance = this; }

    public Transform recipesParent;
    public CraftingSlot recipeSlotPrefab;

    public List<Recipe> recipes;

    public void Initialize()
    {
        LookForNewPossibleRecipes();
    }

    public void LookForNewPossibleRecipes()
    {
        print("Looking for possible recipies");

        ClearSlots();

        if (InventoryManager.Instance.items == null) return;

        for (int i = 0; i < recipes.Count; i++)
        {
            List<Item> tempItems = new List<Item>();
            foreach (Item item in InventoryManager.Instance.items) tempItems.Add(item);

            int contained = 0;
            for(int j = 0; j < recipes[i].ingredients.Count; j++)
            {
                if (tempItems.Remove(recipes[i].ingredients[j]))
                {
                    contained++;
                }
            }

            if(contained == recipes[i].ingredients.Count)
            {
                CraftingSlot recipeSlot = Instantiate(recipeSlotPrefab, recipesParent);
                recipeSlot.Initialize(recipes[i]);
                print(recipeSlot.recipe.name);
            }
        }
    }

    public void Craft(CraftingSlot craftingSlot)
    {
        StartCoroutine(CraftEnumerator(craftingSlot));
    }

    public IEnumerator CraftEnumerator(CraftingSlot craftingSlot)
    {
        for (int i = 0; i < craftingSlot.recipe.ingredients.Count; i++)
        {
            InventoryManager.Instance.GetItem(craftingSlot.recipe.ingredients[i], true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);
        if(InventoryManager.Instance.AddItem(craftingSlot.recipe.product, craftingSlot.recipe.product.defaultData) == false)
        {
            PhotonNetwork.Instantiate(craftingSlot.recipe.product.handlerPrefab.name, transform.position + Vector3.up, transform.rotation);
        }

        LookForNewPossibleRecipes();
    }

    public void ClearSlots()
    {
        for (int i = 0; i < recipesParent.childCount; i++)
        {
            Destroy(recipesParent.GetChild(i).gameObject);
        }
    }

    
}
