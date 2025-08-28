using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LicenseSign : MonoBehaviour
{
    public UILineRenderer line;

    private bool mouseDown;
    private List<Vector2> pointList;
    private Vector2 lastPos;

    void Start()
    {
        mouseDown = false;
        lastPos = new Vector2(0, 0);
        pointList = new List<Vector2>();
        pointList.Add(new Vector2(0, 0));
        line.Points = pointList.ToArray();
    }

    void Update()
    {
        if(mouseDown)
        {
            Vector3 mousePos = Input.mousePosition;
            

            if (mousePos.x < 1151 && mousePos.x > 928 && mousePos.y < 331 && mousePos.y > 226)
            {
                if (lastPos.x == 0)
                {
                    pointList[0] = new Vector2(mousePos.x - 1040, mousePos.y - 277);
                    line.Points = pointList.ToArray();
                    lastPos = pointList[0];
                }
                else if(Vector2.Distance(lastPos, mousePos) > 1)
                {
                    pointList.Add(new Vector2(mousePos.x - 1040, mousePos.y - 277));
                    line.Points = pointList.ToArray();
                    lastPos = pointList[pointList.Count - 1];
                }
            }
        }
    }

    public void OnPointerDown()
    {
        mouseDown = true;
    }

    public void OnPointerUp()
    {
        mouseDown = false;
    }
}
