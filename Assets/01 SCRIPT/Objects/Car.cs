using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarState
{
    Zero,
    Positive90,
    Negative90
}

public class Car : MonoBehaviour
{
    [SerializeField] Vector2 velocity;
    [SerializeField] CarState targetState;
    [SerializeField] CarState state;
    Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = velocity;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "FlipTrigger":
                {
                    switch (state)
                    {
                        case CarState.Zero:
                            {
                                rb2D.velocity = new Vector2(rb2D.velocity.x * -1, 0);
                                break;
                            }
                        case CarState.Positive90:
                            {
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y * -1);
                                break;
                            }
                        case CarState.Negative90:
                            {
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y * -1);
                                break;
                            }
                    }
                    this.transform.localScale = new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y);
                    break;
                }
            case "StopTrigger":
                {
                    rb2D.velocity = Vector2.zero;
                    break;
                }
            case "RedirectTrigger":
                {

                    switch (state)
                    {
                        case CarState.Zero:
                            {

                                if (targetState == CarState.Positive90)
                                {
                                    this.transform.rotation = Quaternion.Euler(0, 0, 90);
                                    rb2D.velocity = new Vector2(0f, rb2D.velocity.x);
                                    state = CarState.Positive90;
                                    col.enabled = false;
                                    StartCoroutine(DelayEnableTrigger(col));
                                }
                                if (targetState == CarState.Negative90)
                                {
                                    this.transform.rotation = Quaternion.Euler(0, 0, -90);
                                    rb2D.velocity = new Vector2(0f, rb2D.velocity.x * -1);
                                    state = CarState.Negative90;
                                    col.enabled = false;
                                    StartCoroutine(DelayEnableTrigger(col));
                                }
                                break;
                            }
                        case CarState.Positive90:
                            {
                                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                                rb2D.velocity = new Vector2(rb2D.velocity.y, 0);
                                state = CarState.Zero;
                                col.enabled = false;
                                StartCoroutine(DelayEnableTrigger(col));
                                break;
                            }
                        case CarState.Negative90:
                            {
                                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                                rb2D.velocity = new Vector2(rb2D.velocity.y * -1, 0);
                                state = CarState.Zero;
                                col.enabled = false;
                                StartCoroutine(DelayEnableTrigger(col));
                                break;
                            }
                    }
                    break;
                }
        }
    }
    IEnumerator DelayEnableTrigger(Collider2D col)
    {
        yield return new WaitForSeconds(10f);
        col.enabled = true;
    }
}
