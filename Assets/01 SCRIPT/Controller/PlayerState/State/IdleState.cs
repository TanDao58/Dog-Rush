using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerStateBase
{
    public override void StartState(PlayerStateManager player)
    {
        // play animation idle
        player.PlayAnim(player.idleDog);
        player.PauseSound(player.runDogsound);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (GameController.Instance.isStartGame)
        {
            player.SwitchState(player.runState, player.runDogsound);
        }
    }
    public override void OnCollisionState(PlayerStateManager player, Collider2D col)
    {

    }
}
