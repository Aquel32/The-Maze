using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Recpie")]
public class Recipe : ScriptableObject
{
    [Header("General")]
    public Item product;
    public List<Item> ingredients;
}