using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupSpoon : MonoBehaviour
{
    public SoupGame game;
    public RectTransform spoon;

    private RectTransform spoonP;
    private RectTransform Rtransform;
    private RectTransform parentTransform;
    void Start()
    {
        spoonP = spoon.transform.parent.GetComponent<RectTransform>();
        parentTransform = transform.parent.GetComponent<RectTransform>();
        Rtransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Vector2.Distance(parentTransform.anchoredPosition + Rtransform.anchoredPosition, spoonP.anchoredPosition + spoon.anchoredPosition) < 10)
        {
            game.TriggerPoint(this.gameObject);
        }
    }
}
