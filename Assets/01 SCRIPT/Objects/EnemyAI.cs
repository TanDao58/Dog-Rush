using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Player Check Properties")]
    [SerializeField] List<GameObject> listTargetInScene;
    [SerializeField] Transform target;
    [SerializeField] float RadiusCheck = 3f;

    [Header("Enemy Properties")]
    [SerializeField] float speed;
    [SerializeField] float nextWaypointDistance = 3f;
    [SerializeField] Transform enemyGFX;

    Path path;
    int currentWayPoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in g)
        {
            listTargetInScene.Add(go);
        }
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.isStartGame)
        {
            Collider2D[] lstCol = Physics2D.OverlapCircleAll(this.transform.position, RadiusCheck);
            if (lstCol.Length > 0)
            {
                foreach (Collider2D col in lstCol)
                {
                    if (col.GetComponent<Collider2D>() != null)
                        if (listTargetInScene.Contains(col.gameObject))
                        {
                            target = col.transform;
                            break;
                        }
                }
            }
            else
            {
                target = null;
                return;
            }
            if (path == null)
            {
                return;
            }
            if (currentWayPoint >= path.vectorPath.Count)
            {
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWaypointDistance)
            {
                currentWayPoint++;
            }
            if (rb.velocity.x >= 0.01f)
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (rb.velocity.x <= -0.01f)
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RadiusCheck);
    }
}
