using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horzInput = Input.GetAxisRaw("Horizontal");
        float vertInput = Input.GetAxisRaw("Vertical");

        //이동 방향벡터 계산
        Vector3 moveDirection = new Vector3(horzInput, vertInput, 0f).normalized;

        transform.position += moveDirection * speed * Time.deltaTime;
    }

}
