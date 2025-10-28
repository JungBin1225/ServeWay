using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLicense : MonoBehaviour
{
    private InteractionWindow interaction;
    private bool getAble;

    public GameObject license;

    void Start()
    {
        interaction = FindObjectOfType<InteractionWindow>();
        getAble = false;
    }

    void Update()
    {
        if(getAble && Input.GetKeyDown(KeyCode.F))
        {
            license.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getAble = true;
            interaction.SetIngredGetAble(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getAble = false;
            interaction.SetIngredGetAble(false);
        }
    }
}
