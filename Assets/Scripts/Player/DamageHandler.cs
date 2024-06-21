using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviourPunCallbacks, IDamageable
{
    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("DamageRPC", GetComponent<PlayerReference>().referencedPlayer.photonPlayer, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Mob));
    }

    [PunRPC]
    public void DamageRPC(int damage)
    {
        HealthSystem.Instance.RemoveHealth(damage);
    }
}
