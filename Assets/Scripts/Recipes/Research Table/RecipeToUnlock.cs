using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeToUnlock : MonoBehaviour
{
    [SerializeField] private Recipe recipe;
    [SerializeField] private RecipeToUnlock[] recipesToShow;

    private ExperienceSystem experienceSystem;
    private Button unlockButton;

    public bool unlocked;

    private void Start()
    {
        unlocked = false;

        experienceSystem = Player.myPlayer.playerObject.GetComponent<ExperienceSystem>();
        unlockButton = GetComponentInChildren<Button>();

        transform.Find("RecipeIcon").GetComponent<Image>().sprite = recipe.product.image;
        transform.Find("ProductNameText").GetComponent<TextMeshProUGUI>().text = recipe.product.name;
        transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = "COST: " + recipe.expToUnlock;
        unlockButton.onClick.AddListener(Unlock);
    }

    public void Unlock()
    {
        if (experienceSystem.experience < recipe.expToUnlock) return;
        experienceSystem.ChangeExperience(-recipe.expToUnlock);

        unlocked = true;

        unlockButton.interactable = false;
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "UNLOCKED";
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;

        if(recipe.recipeType == AdditionalPanelType.Crafting) experienceSystem.craftingSystem.recipes.Add(recipe);
        if(recipe.recipeType == AdditionalPanelType.Furance) experienceSystem.furanceSystem.recipes.Add(recipe);
        if(recipe.recipeType == AdditionalPanelType.Anvil) experienceSystem.anvilSystem.recipes.Add(recipe);

        for(int i = 0; i < recipesToShow.Length; i++)
        {
            recipesToShow[i].gameObject.SetActive(true);
        }
    }
}
