using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Items/Tool")]
public class Tool : Item
{
    [Header("Tool")]
    public int damage;
    public int durability;
    public float cooldownTime = 0;
    public int effectiveRange = 0;
    public ToolType toolType;
}

public enum ToolType { Sword, Axe, Pickaxe, Other }
