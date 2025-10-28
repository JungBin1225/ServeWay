using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private string sceneName;

    public Image bar;

    void Start()
    {
        sceneName = GameManager.gameManager.GetNextStage();
        bar.fillAmount = 0;

        StartCoroutine(LoadScene());
    }

    void Update()
    {
        
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while(bar.fillAmount != 1.0f)
        {
            if(op.progress < 0.9f)
            {
                bar.fillAmount = op.progress;
            }
            else
            {
                bar.fillAmount = 1.0f;
            }
            
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        op.allowSceneActivation = true;
    }
}
