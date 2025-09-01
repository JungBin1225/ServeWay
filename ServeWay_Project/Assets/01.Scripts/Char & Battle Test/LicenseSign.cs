using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;

public class LicenseSign : MonoBehaviour
{
    public GameObject signPrefab;
    public GameObject gameClear;
    public GameObject placeHolder;
    public GameObject button;
    public GameObject stamp;

    private UILineRenderer line;
    private int signAmount;
    private bool mouseDown;
    private List<Vector2> pointList;
    private Vector2 lastPos;
    private bool signAble;
    private bool playingStamp;

    void Start()
    {
        mouseDown = false;
        signAble = true;
        playingStamp = false;
        signAmount = 0;

        lastPos = new Vector2(0, 0);
        pointList = new List<Vector2>();
        pointList.Add(new Vector2(0, 0));

        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        if(gameClear.activeSelf)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = "돌아가기";
            signAble = false;
            playingStamp = false;
        }
    }

    void Update()
    {
        if(mouseDown && signAmount <= 20 && pointList.Count <= 3000 && signAble)
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

    private IEnumerator PlayStamp()
    {
        playingStamp = true;
        Animator anim = stamp.GetComponent<Animator>();
        
        stamp.SetActive(true);
        anim.SetTrigger("start");
        yield return new WaitForSecondsRealtime(0.1f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).length < anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSecondsRealtime(0.5f);

        gameClear.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerDown()
    {
        mouseDown = true;
        placeHolder.SetActive(false);

        if(signAmount <= 20)
        {
            GameObject sign = Instantiate(signPrefab, transform);
            signAmount++;

            line = sign.GetComponent<UILineRenderer>();
            lastPos = new Vector2(0, 0);
            pointList = new List<Vector2>();
            pointList.Add(new Vector2(0, 0));
        }
    }

    public void OnPointerUp()
    {
        mouseDown = false;
        button.SetActive(true);

        lastPos = new Vector2(0, 0);
        pointList = new List<Vector2>();
        pointList.Add(new Vector2(0, 0));
    }

    public void OnReceiveClicked()
    {
        if(signAble)
        {
            signAble = false;
            StartCoroutine(PlayStamp());
        }
        else if(!playingStamp)
        {
            transform.parent.gameObject.SetActive(false);
        }
        
    }
}
