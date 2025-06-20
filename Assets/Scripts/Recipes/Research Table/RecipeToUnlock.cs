using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeToUnlock : MonoBehaviour
{
    public Recipe recipe;
    [SerializeField] private RecipeToUnlock[] requirements;

    private Button unlockButton;

    public bool unlocked;

    public void Initialize()
    {
        unlockButton = GetComponentInChildren<Button>();

        transform.Find("RecipeIcon").GetComponent<Image>().sprite = recipe.product.image;
        transform.Find("ProductNameText").GetComponent<TextMeshProUGUI>().text = recipe.product.name;
        transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = "COST: " + recipe.expToUnlock;
        unlockButton.onClick.AddListener(UnlockButton);

        if(unlocked == true)
        {
            SwtichState(true);
        }
    }

    public void UnlockButton()
    {
        if (ExperienceSystem.Instance.experience < recipe.expToUnlock) return;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i].unlocked == false)
            {
                return;
            }
        }

        Unlock();
    }

    public void Unlock()
    {
        ExperienceSystem.Instance.ChangeExperience(-recipe.expToUnlock);
        RecipeManager.Instance.UnlockRecipe(recipe);
        SwtichState(true);
    }

    public void SwtichState(bool newState)
    {
        unlocked = newState;
        unlockButton.interactable = !newState;
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = newState ? "UNLOCKED" : "UNLOCK";
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = newState ? Color.green : Color.white;
    }
}
