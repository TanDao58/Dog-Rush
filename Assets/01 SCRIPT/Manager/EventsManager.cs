using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    [Header("Win Panel")]
    [SerializeField] GameObject WinPanel;
    [SerializeField] Image WinIcons;

    [Header("Lose Panel")]
    [SerializeField] GameObject LosePanel;
    [SerializeField] Image LoseIcons;

    [Header("Sound End game")]
    [SerializeField] AudioClip SoundWinGame;
    [SerializeField] AudioClip SoundLoseByKilled;

    [Header("Animation properties")]
    [SerializeField] float timerAnim = 0f;

    public ButtonManager button;
    bool isEndCheckPlayer = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameobjectsWithEvents.OnPlaySoundGameObjects += PlaySoundObjects;
        GameobjectsWithEvents.OnTriggerStayed += TriggerGameOver;
    }

    private void OnDisable()
    {
        isEndCheckPlayer = false;
        GameobjectsWithEvents.OnPlaySoundGameObjects -= PlaySoundObjects;
        GameobjectsWithEvents.OnTriggerStayed -= TriggerGameOver;
    }

    private void Update()
    {
        if (!isEndCheckPlayer && GameController.Instance.listPlayer.Count > 0)
        {
            foreach (player p in GameController.Instance.listPlayer)
            {
                if (p.isKilledbyObjects)
                {
                    StartCoroutine(DelayEndPanel(timerAnim, false, SoundLoseByKilled));
                    return;
                }
            }
            foreach (player p in GameController.Instance.listPlayer)
            {
                if (!p.isCompleteMission)
                    return;
            }
            StartCoroutine(DelayEndPanel(timerAnim, true, SoundWinGame));
        }
    }
    void PlaySoundObjects(AudioClip sound)
    {
        SoundManager.Instance.PlaySound(sound.name, true);
    }
    void TriggerGameOver(GameObject[] g, AudioClip soundEnd, Collider2D currentcol, Collider2D col, bool isWin, AudioClip soundEndGame, Sprite imgEndGame)
    {
        if (col.CompareTag("Player"))
        {
            if (!isWin)
            {
                foreach (player p in GameController.Instance.listPlayer)
                {
                    p.isKilledbyObjects = true;
                }

                SoundManager.Instance.PlaySound(soundEnd.name);
                SoundLoseByKilled = soundEndGame;
                LoseIcons.overrideSprite = imgEndGame;

                if (currentcol.CompareTag("DCCather"))
                {
                    col.gameObject.SetActive(false);
                    g[0].SetActive(false);
                    currentcol.transform.GetComponent<Rigidbody2D>().isKinematic = true;
                    switch (col.gameObject.GetComponent<player>().lineController.typeplayer)
                    {
                        case TypePlayer.Red:
                            {
                                g[1].SetActive(true);
                                break;
                            }
                        case TypePlayer.Blue:
                            {
                                g[2].SetActive(true);
                                break;
                            }
                    }
                    return;
                }

                g[0].SetActive(true);
                if (currentcol.CompareTag("Car"))
                    currentcol.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                g[0].transform.position = col.transform.position;
                g[0].transform.rotation = col.transform.rotation;
            }
            else
            {
                player temp = col.GetComponent<player>();
                if (temp == null)
                    return;
                if (!temp.lineController.EndCollider.Contains(currentcol))
                    return;
                if (!temp.isCompleteMission)
                    return;
                g[0].SetActive(true);
                SoundManager.Instance.PlaySound(soundEnd.name);
                currentcol.gameObject.SetActive(false);
            }
            col.gameObject.SetActive(false);
        }
        else
            return;
    }
    IEnumerator DelayEndPanel(float timer, bool isWin, AudioClip sound)
    {
        isEndCheckPlayer = true;
        yield return new WaitForSeconds(timer);
        if (!button.isReturnHome)
        {
            SoundManager.Instance.StopAllSoundExcept("1. Background 2");
            SoundManager.Instance.PlaySound(sound.name);
        }

        if (!isWin)
        {
            WinPanel.SetActive(false);
            LosePanel.SetActive(true);
            UIManager.Instance.PanelFadeIn(LosePanel.transform.GetChild(0).GetComponent<CanvasGroup>(), LosePanel.transform.GetChild(0).GetComponent<RectTransform>());
        }
        else
        {
            LosePanel.SetActive(false);
            WinPanel.SetActive(true);
            UIManager.Instance.PanelZoomIn(WinPanel.transform.GetChild(0).GetComponent<CanvasGroup>(), WinPanel.transform.GetChild(0).GetComponent<RectTransform>());
        }
    }
}
