using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerStateBase
{
    public override void StartState(PlayerStateManager player)
    {
        // play animation dead
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void OnCollisionState(PlayerStateManager player, Collider2D col)
    {

    }
}
