using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : Singleton<GameController>
{
    [Header("Player Settings")]
    [SerializeField] public List<player> listPlayer;
    [SerializeField] float TimeCompleteGame = 0;
    [SerializeField] Transform LevelSpawn;

    [SerializeField] float TimeScale = 0;

    #region value check
    public bool isStartGame = false;
    #endregion

    private void OnEnable()
    {
        if (LevelSpawn.childCount != 0)
            for (int i = 0; i < LevelSpawn.GetChild(0).GetChild(0).childCount; i++)
            {
                listPlayer.Add(LevelSpawn.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<player>());
            }
        SoundManager.Instance.PlaySound("5. Tiếng Chó Gầm Gừ", true);
        SoundManager.Instance.PlaySound("2. Tiếng Trêu Chó 2", true);
    }


    private void OnDisable()
    {
        isStartGame = false;
        listPlayer.Clear();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SoundAction("5. Tiếng Chó Gầm Gừ");
            SoundManager.Instance.SoundAction("2. Tiếng Trêu Chó 2");
        }
    }
    private void Update()
    {
        if (!isStartGame)
        {
            foreach (player character in listPlayer)
            {
                if (character.lineController.tempLine != null)
                {
                    character.smoothTime = character.lineController.tempLine.GetLineLength() * TimeScale / TimeCompleteGame;
                    continue;
                }
                else
                    return;
            }
            isStartGame = true;
            SoundManager.Instance.SoundAction("5. Tiếng Chó Gầm Gừ");
            SoundManager.Instance.SoundAction("2. Tiếng Trêu Chó 2");
        }
    }
}
