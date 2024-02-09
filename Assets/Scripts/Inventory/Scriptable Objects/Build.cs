using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Items/Build")]
public class Build : Item
{
    [Header("Building")]
    public int health;
}
