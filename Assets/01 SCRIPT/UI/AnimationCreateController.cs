using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Move,
    Rotate,
    Scale,
}

public class AnimationCreateController : MonoBehaviour
{

    public AnimationType type;
    [SerializeField] float AnimDuration;
    [SerializeField] Ease animEase;
    [SerializeField] int loop;
    [SerializeField] LoopType loopType;

    [Header("The move components")]
    [SerializeField] float moveY;

    [Header("The rotate components")]
    [SerializeField] RotateMode rotateMode;
    [SerializeField] Vector3 numRotate;

    [Header("The scale components")]
    [SerializeField] Vector3 numScale;

    void Start()
    {
        switch (type)
        {
            case AnimationType.Move:
                {
                    this.transform.DOMoveY(moveY, AnimDuration).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.Rotate:
                {
                    this.transform.DORotate(numRotate, AnimDuration, rotateMode).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.Scale:
                {
                    this.transform.DOScale(numScale, AnimDuration).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
        }
    }

}
