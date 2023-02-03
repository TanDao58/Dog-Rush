using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypePlayer
{
    Blue,Red
}

public class LinesDrawer : MonoBehaviour
{
    [SerializeField] GameObject linePrefabs;
    [SerializeField] public TypePlayer typeplayer;

    [Header("Line Properties")]
    [SerializeField] Gradient lineColor;
    [SerializeField] float linePointsMinDistance;
    [SerializeField] float lineWidth;

    [Header("Check Input Properties")]
    [SerializeField] Collider2D playerCollider;
    [SerializeField] public List<Collider2D> EndCollider;
    [SerializeField] float CheckColliderRadius;
    [SerializeField] LayerMask cantDrawOverLayer;
    [SerializeField] LineRenderer warningLine;

    [Header("List Line Transform")]
    [SerializeField] Transform LineController;

    [Header("Public Infomation")]
    public Line tempLine;


    RaycastHit2D hit;
    Line currentLine;
    Camera cam;

    private void Awake()
    {
        LineController = GameObject.FindGameObjectWithTag("ListLineInScene").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Input.multiTouchEnabled = false;
        for (int i = 0; i < LineController.childCount; i++)
        {
            Destroy(LineController.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] listColInput = Physics2D.OverlapCircleAll(cam.ScreenToWorldPoint(Input.mousePosition), CheckColliderRadius);
            foreach (Collider2D col in listColInput)
            {
                if (col == playerCollider)
                {
                    BeginDraw();
                    break;
                }
            }
        }
        if (currentLine != null)
            Draw();

        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }
    // Begin Draw
    void BeginDraw()
    {
        if (tempLine == null)
        {
            currentLine = Instantiate(linePrefabs, LineController).GetComponent<Line>();
            currentLine.SetLineColor(lineColor);
            currentLine.SetPointsMinDistance(linePointsMinDistance);
            currentLine.SetLineWidth(lineWidth);
        }
    }
    //Draw
    void Draw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        if (currentLine.GetLineRendererCount() > 0)
        {
            hit = Physics2D.Linecast(currentLine.GetLastPoint(), mousePosition, cantDrawOverLayer);
            if (hit)
            {
                warningLine.gameObject.SetActive(true);
                warningLine.positionCount = 2;
                warningLine.SetPosition(0, currentLine.GetLastPoint());
                warningLine.SetPosition(1, mousePosition);
                return;
            }
            else
            {
                warningLine.gameObject.SetActive(false);
            }
        }
        currentLine.AddPoint(mousePosition);
    }
    //End Draw
    void EndDraw()
    {
        if (currentLine != null)
        {
            if (currentLine.pointsCount < 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                Collider2D[] listColLastPoint = Physics2D.OverlapCircleAll(currentLine.GetLastPoint(), CheckColliderRadius);
                foreach (Collider2D col in listColLastPoint)
                {
                    if (EndCollider.Contains(col))
                    {
                        col.enabled = false;
                        tempLine = currentLine;
                        currentLine = null;
                        return;
                    }
                }
                Destroy(currentLine.gameObject);
                warningLine.gameObject.SetActive(false);
            }
        }
    }
}
