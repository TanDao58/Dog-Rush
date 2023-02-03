using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagInGame
{
    Player,
    KilledPlayer,
    FightedPlayer
}

public abstract class PlayerStateBase
{
    public abstract void StartState(PlayerStateManager player); 
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionState(PlayerStateManager player, Collider2D col);
}

