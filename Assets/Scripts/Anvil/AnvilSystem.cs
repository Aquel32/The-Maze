using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilSystem : MonoBehaviour, ITable
{
    public static AnvilSystem Instance;
    public void Awake() { Instance = this; }

    public Transform recipesParent;
    public AnvilSlot recipeSlotPrefab;

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
                AnvilSlot recipeSlot = Instantiate(recipeSlotPrefab, recipesParent);
                recipeSlot.Initialize(recipes[i]);
                print(recipeSlot.recipe.name);
            }
        }
    }

    public void Craft(AnvilSlot craftingSlot)
    {
        StartCoroutine(CraftEnumerator(craftingSlot));
    }

    public IEnumerator CraftEnumerator(AnvilSlot anvilSlot)
    {
        for (int i = 0; i < anvilSlot.recipe.ingredients.Count; i++)
        {
            InventoryManager.Instance.GetItem(anvilSlot.recipe.ingredients[i], true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        if (InventoryManager.Instance.AddItem(anvilSlot.recipe.product, anvilSlot.recipe.product.defaultData) == false)
        {
            PhotonNetwork.Instantiate(anvilSlot.recipe.product.handlerPrefab.name, transform.position + (Vector3.up * 3), transform.rotation);
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
