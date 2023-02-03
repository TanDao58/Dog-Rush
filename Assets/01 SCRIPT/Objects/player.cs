using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] public Vector2 velocity;

    public float smoothTime = 0;
    public bool isCompleteMission = false;
    public bool isKilledbyObjects = false;


    [SerializeField] float distanceCheck = 0;
    [SerializeField] public LinesDrawer lineController;

    int posIndex = 0;

    private void FixedUpdate()
    {
        if (lineController == null)
            return;
        if (lineController.tempLine == null)
            return;
        if (posIndex >= lineController.tempLine.pointsCount)
        {
            foreach (Collider2D line in lineController.EndCollider)
            {
                if (!line.enabled)
                    line.enabled = true;
            }
            isCompleteMission = true;
            return;
        }
        if (posIndex < lineController.tempLine.pointsCount && GameController.Instance.isStartGame)
        {
            //transform.position = Vector2.MoveTowards(transform.position, lineController.tempLine.points[posIndex] , smoothTime);
            if (!isKilledbyObjects)
            {
                //transform.position = Vector2.SmoothDamp(transform.position, lineController.tempLine.points[posIndex], ref velocity, smoothTime);
                transform.position = Vector2.MoveTowards(transform.position, lineController.tempLine.points[posIndex], smoothTime);
                if (Vector2.Distance(transform.position, lineController.tempLine.points[posIndex]) < distanceCheck)
                {
                    posIndex++;
                }
                if (posIndex < lineController.tempLine.pointsCount)
                {
                    if ((Mathf.Abs(transform.localPosition.x) - Mathf.Abs(Camera.main.ScreenToWorldPoint(lineController.tempLine.points[posIndex]).x)) >= 4f)
                        this.transform.localScale = new Vector2(1f, 1f);
                    else
                        this.transform.localScale = new Vector2(-1f, 1f);
                }
            }
        }
    }
}
