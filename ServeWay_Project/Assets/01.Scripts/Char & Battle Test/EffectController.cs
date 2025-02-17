using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EffectController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyEffect());
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    IEnumerator DestroyEffect()
    {
        float time = 0;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip)
            {
                time = clip.length / state.speed;
            }
        }

        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}
