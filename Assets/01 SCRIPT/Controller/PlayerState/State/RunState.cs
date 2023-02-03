using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerStateBase
{
    public override void StartState(PlayerStateManager player)
    {
        // play animation run
        player.PlayAnim(player.runDog);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.mainplayer.isCompleteMission)
        {
            if (player.mainplayer.isKilledbyObjects)
                player.PauseSound(player.runDogsound);
        }
        else
            player.PauseSound(player.runDogsound);
    }

    public override void OnCollisionState(PlayerStateManager player, Collider2D col)
    {

    }
}
