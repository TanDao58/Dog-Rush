using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectsWithEvents : MonoBehaviour
{
    public static event Action<GameObject[], AudioClip, Collider2D, Collider2D, bool, AudioClip, Sprite> OnTriggerStayed;
    public static event Action<AudioClip> OnPlaySoundGameObjects;

    [SerializeField] bool IsWinGame = false;
    [SerializeField] GameObject[] animationGameObjects;

    [Header("Sound game objects")]
    [SerializeField] AudioClip soundObjectsStart;
    [SerializeField] AudioClip soundObjectsEnd;

    [Header("Design End Panel")]
    [SerializeField] AudioClip soundEndPanel;
    [SerializeField] Sprite imgEndPanel;

    [Header("Check Player End Game to STOP")]
    public bool isCheck = false;

    private void Start()
    {
        if (soundObjectsStart != null)
            OnPlaySoundGameObjects(soundObjectsStart);
    }
    private void Update()
    {
        if (isCheck)
        {
            foreach (player p in GameController.Instance.listPlayer)
            {
                if (p.isKilledbyObjects)
                {
                    this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    isCheck = false;
                    return;
                }
            }

        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col != null && soundObjectsEnd != null)
        {
            OnTriggerStayed(animationGameObjects, soundObjectsEnd, this.GetComponent<Collider2D>(), col, IsWinGame, soundEndPanel, imgEndPanel);
        }
    }

}
