using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSetting : MonoBehaviour
{
    public GameObject optionMenu;

    public void OpenMenu()
    {
        Time.timeScale = 0;

        optionMenu.SetActive(true);
        optionMenu.GetComponent<OptionMenu>().InitValue();
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        optionMenu.GetComponent<OptionMenu>().GoBack();

        optionMenu.SetActive(false);
    }
}
