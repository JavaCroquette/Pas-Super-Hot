﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float dashForce = 0f;
    [SerializeField] private float gravityForce = 9.81f;
    [SerializeField] private float flyDelay = 1f;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private Transform playerTransform = default;
    [SerializeField] private GameObject feet = default;
    [SerializeField] private LayerMask groundLayer = default;
    public float rotationSpeed = 50f;

    private Vector3 gravityDir = Vector3.down;
    private bool isGrounded = true;
    private bool useGravity = true;
    private bool hasPowers = true;


    private void FixedUpdate()
    {
        // Apply gravity
        if (useGravity)
        {
            if (isGrounded && hasPowers)
                rb.AddRelativeForce(Vector3.down * gravityForce * Time.fixedDeltaTime);
            else
                rb.AddForce(Vector3.down * gravityForce * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        playerTransform.Rotate(0f,
            Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime,
            0f);
        if (Physics.OverlapSphere(feet.transform.position, .5f, groundLayer).Length > 0)
            isGrounded = true;
        else
            isGrounded = false;
    }

    public void Dash(Vector3 dir)
    {
        if (!isGrounded)
            return;

        rb.AddForce(dir * dashForce, ForceMode.Impulse);
        useGravity = false;
        StartCoroutine(nameof(ResetUseGravity), flyDelay);
    }

    public void LosePowers()
    {
        hasPowers = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            ContactPoint contactPoint = collision.GetContact(0);
            rb.velocity = Vector3.zero;
            playerTransform.position = contactPoint.point - contactPoint.normal * feet.transform.localPosition.y;
            playerTransform.rotation = Quaternion.Euler(collision.transform.rotation.eulerAngles + Vector3.right * 0); // BUG TO RESOLVE HERE
        }
    }

    private IEnumerator ResetUseGravity(float delay)
    {
        yield return new WaitForSeconds(delay);
        useGravity = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feet.transform.position, .1f);
    }
}
