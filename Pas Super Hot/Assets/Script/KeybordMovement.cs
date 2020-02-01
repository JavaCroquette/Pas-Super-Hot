using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private float speed = 2000f;
    public Rigidbody body;
    private float jumpstrenght = 1000f;
    void Start()
    {
        body = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float backforward = Input.GetAxis("Horizontal");
        float leftright = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");
        
        Vector3 move = (transform.right * backforward + transform.forward * leftright) * speed + transform.up * jump * jumpstrenght;
        body.velocity = move * Time.deltaTime;
        
        
        

    }
}
