using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigidbody;
    private Vector3 mousePos;
    private float coolTime;
    private Vector2 moveVel;

    public float speed;
    public float chargeSpeed;
    public float chargeLength;
    public float chargeCooltime;

    public bool controllAble;
    public bool isCharge;

    void Start()
    {
        controllAble = true;
        isCharge = false;
        coolTime = 0;

        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if(controllAble)
        {
            mousePos = UpdateMousePos();
            UpdateDirection(mousePos);

            Move();
        }

        if(coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
        else if(coolTime < 0)
        {
            coolTime = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(coolTime == 0)
            {
                StartCoroutine(UseCharge());
            }
            else
            {
                Debug.Log("Cool Time!");
            }
        }
        
    }

    public Vector3 UpdateMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void UpdateDirection(Vector3 mouse)
    {
        if (Mathf.Abs(transform.position.x - mouse.x) > Mathf.Abs((transform.position.y - (transform.localScale.y / 2)) - mouse.y))
        {
            if (transform.position.x - mouse.x < 0)
            {
                anim.SetInteger("direction", 2);
            }
            else
            {
                anim.SetInteger("direction", 4);
            }
        }
        else
        {
            if ((transform.position.y - (transform.localScale.y / 2)) - mouse.y < 0)
            {
                anim.SetInteger("direction", 1);
            }
            else
            {
                anim.SetInteger("direction", 3);
            }
        }
    }

    private void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        moveVel = new Vector2(xMove, yMove) * speed;
        rigidbody.velocity = moveVel;

        if (rigidbody.velocity.magnitude == 0 && controllAble)
        {
            anim.SetBool("isMove", false);
        }
        else
        {
            anim.SetBool("isMove", true);
        }
    }

    private IEnumerator UseCharge()
    {
        controllAble = false;
        Vector2 chargeVel = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        /*Vector2 chargeVel = moveVel.normalized;
        if(chargeVel == Vector2.zero)
        {
            chargeVel = new Vector2(1, 0).normalized;
        }*/

        rigidbody.velocity = chargeVel * -2;

        yield return new WaitForSeconds(0.25f); //¼±µô

        isCharge = true;

        //rigidbody.velocity = chargeVel;
        rigidbody.AddForce(chargeVel * chargeSpeed * 0.2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(chargeLength); //µ¹Áø

        //GameManager.gameManager.mission.CheckCharge();
        isCharge = false;

        rigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f); //ÈÄµô

        controllAble = true;
        coolTime = chargeCooltime;
    }
}
