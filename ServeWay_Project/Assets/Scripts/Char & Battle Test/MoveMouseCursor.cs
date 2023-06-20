using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMouseCursor : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 mousePos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        mousePos = Input.mousePosition;

        rectTransform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
