using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTime : MonoBehaviour
{
    public Transform building;
    private float myVelocity;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform chuncks in building)
        {
            Animator anim = chuncks.GetComponent<Animator>();
            //anim.runtimeAnimatorController.animationClips. = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
