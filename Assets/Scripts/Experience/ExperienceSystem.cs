using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    public int experience;

    public CraftingSystem craftingSystem;
    public AnvilSystem anvilSystem;
    public FuranceSystem furanceSystem;

    [SerializeField] private TextMeshProUGUI experienceText;

    public void ChangeExperience(int number)
    {
        experience += number;
        if(experience < 0) experience = 0;

        UpdateExperienceText();
    }

    public void UpdateExperienceText()
    {
        experienceText.text = "Experience: " + experience;
    }
}