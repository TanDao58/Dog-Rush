using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] float fadeTimeIn = 1f, fadeTimeOut = 0.5f;
    [SerializeField] Ease fadeEaseIn, fadeEaseOut;



    public void PanelFadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -150f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTimeIn, false).SetEase(fadeEaseIn);
        canvasGroup.DOFade(1, fadeTimeIn);
    }
    public void PanelFadeOut(CanvasGroup canvasGroup, RectTransform rectTransform, GameObject g)
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -150f), fadeTimeOut, false).SetEase(fadeEaseOut);
        canvasGroup.DOFade(0, fadeTimeOut);
        StartCoroutine(DelayDisableGameobjects(g));
    }

    public void PanelZoomIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rectTransform.DOScale(new Vector3(1f, 1f, 1f), fadeTimeIn).SetEase(fadeEaseIn);
        canvasGroup.DOFade(1, fadeTimeIn);
    }

    IEnumerator DelayDisableGameobjects(GameObject g)
    {
        yield return new WaitForSeconds(fadeTimeOut);
        g.SetActive(false);
    }
}
