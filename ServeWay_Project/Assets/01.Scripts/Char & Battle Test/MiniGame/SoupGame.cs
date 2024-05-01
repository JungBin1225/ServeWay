using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public GameObject pot;
    public GameObject spoon;

    private bool spoonDown;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void OnEnable()
    {
        spoonDown = false;

        minX = pot.GetComponent<RectTransform>().transform.position.x - (pot.GetComponent<RectTransform>().sizeDelta.x / 2);
        maxX = pot.GetComponent<RectTransform>().transform.position.x + (pot.GetComponent<RectTransform>().sizeDelta.x / 2);
        minY = pot.GetComponent<RectTransform>().transform.position.y - (pot.GetComponent<RectTransform>().sizeDelta.y / 2);
        maxY = pot.GetComponent<RectTransform>().transform.position.y + (pot.GetComponent<RectTransform>().sizeDelta.y / 2);

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(spoonDown)
        {
            Vector3 mousePos = Input.mousePosition;

            if(mousePos.x >= minX && mousePos.x <= maxX && mousePos.y >= minY && mousePos.y <= maxY)
            {
                spoon.GetComponent<RectTransform>().transform.position = mousePos;
            }
        }
    }

    public void OnPointerDown()
    {
        spoonDown = true;
    }

    public void OnPointerUp()
    {
        spoonDown = false;
    }

    public void OnStartClicked()
    {
        //StartCoroutine(GameStart());
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
