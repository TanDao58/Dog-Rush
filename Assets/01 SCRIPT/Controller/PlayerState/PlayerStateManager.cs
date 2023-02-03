using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerStateBase currentState;

    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public DeadState deadState = new DeadState();
    public FightState fightState = new FightState();

    public AnimationClip idleDog;
    public AnimationClip runDog;


    public AudioClip runDogsound;

    public player mainplayer;

    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;
        currentState.StartState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance != null)
            currentState.UpdateState(this);
    }

    public void PlayAnim(AnimationClip player)
    {
        this.GetComponent<Animator>().Play(player.name);
    }

    public void PauseSound(AudioClip sound)
    {
        SoundManager.Instance.SoundAction(sound.name);
    }

    public void SwitchState(PlayerStateBase player, AudioClip sound)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(sound.name, true);
        currentState = player;
        currentState.StartState(this);
    }
}
