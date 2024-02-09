using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    private void Update()
    {
        
    }

    public int experience;

    public void ChangeExperience(int number)
    {
        experience += number;
        if(experience < 0) experience = 0;
    }
}
