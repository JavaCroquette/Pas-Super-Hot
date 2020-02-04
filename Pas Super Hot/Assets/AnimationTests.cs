using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTests : MonoBehaviour
{
    //private Animation animation;
    private Animator m_Animator;
    private bool hasAnimation = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        //Debug.Log(m_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash);

        //hasAnimation = gameObject.TryGetComponent<Animation>(out animation);
        //if (hasAnimation)
        //    animation.wrapMode = WrapMode.ClampForever;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        m_Animator.SetFloat("Speed", move);
        Debug.Log(m_Animator.GetFloat("Speed"));
        if (hasAnimation)
        {
            //AnimationState animationState = animation.GetHashCode();
            //if (animationState.time == animationState.length) animationState.speed = -1;
            //if (animationState.time == 0f) animationState.speed = 1;
        }
    }
}
