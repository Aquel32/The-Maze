using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Items/Medical")]
public class Medical : Item
{
    [Header("Medical")]
    public int healthToAdd = 0;
}
