using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float gravityForce = 1255.68f;
    [SerializeField] private float flyDelay = 1f;
    [SerializeField] private float turnDistance = 10f;
    [SerializeField] private float multiplierAnimHigh = 0.1f;
    [SerializeField] private float multiplierAnimMid = 0.04f;
    [SerializeField] private float multiplierAnimLow = 0.01f;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private Transform playerTransform = default;
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private GameObject feet = default;
    [SerializeField] private LayerMask groundLayer = default;

    [SerializeField] private AudioSource jump1, jump2;
    public float rotationSpeed = 50f;

    private float multiplierAnim = 0f;
    private Vector3 gravityDir = Vector3.down;
    private Vector3 forwardCam = Vector3.zero;
    [SerializeField] private bool isGrounded = true;
    private bool useGravity = true;
    private bool hasPowers = true;
    [SerializeField] private bool isDashing = false;

    private Animator[] animators;
    [SerializeField] private bool isMoving = false;

    private void Awake()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        //Debug.Log(platforms.Length);
        animators = new Animator[platforms.Length - 4];//-4 is for 4 walls
        int i = 0;
        foreach (GameObject platform in platforms)
        {
            bool isAnim = platform.TryGetComponent<Animator>(out animators[i]);
            //Debug.Log("isAnim = " + isAnim + (isAnim ? "" : " name" + platform.name));
            if (isAnim) i++;
        }

        if (rb == default)
            rb = GetComponent<Rigidbody>();
        if (playerTransform == default)
            playerTransform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        // Apply gravity
        if (useGravity)
        {
            if (isGrounded && hasPowers)
                rb.AddForce(gravityDir * gravityForce * Time.fixedDeltaTime);
            else
                rb.AddForce(Vector3.down * gravityForce * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        playerTransform.Rotate(0f,
            Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime,
            0f);
        isGrounded = Physics.OverlapSphere(feet.transform.position, .5f, groundLayer).Length > 0;
        if (isGrounded && !isDashing)
        {
            Vector3 newVelocity = speed * Time.deltaTime * (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
            isMoving = !newVelocity.Equals(Vector3.zero);
            if (isMoving)
            {
                rb.velocity = newVelocity;
            }
        }
        if (isDashing)
        {
            if (Physics.Raycast(playerTransform.position, forwardCam, out RaycastHit hitinfo, turnDistance))
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                Vector3.RotateTowards(hitinfo.normal, forwardCam, 90f * Mathf.Deg2Rad, 1f),
                hitinfo.normal);

                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, (turnDistance - hitinfo.distance) / turnDistance);
            }
            else
            {
                forwardCam = rb.velocity.normalized;
            }
        }
        //Debug.Log(multiplierAnim);
        ChangeAnimationSpeed();
    }

    private void ChangeAnimationSpeed()
    {
        if (isDashing && multiplierAnim != multiplierAnimHigh)
        {
            multiplierAnim = multiplierAnimHigh;
            foreach (Animator anim in animators)
            {
                anim.speed = multiplierAnim;
            }
        }
        else if (!isDashing && !isMoving && multiplierAnim != multiplierAnimLow)
        {
            multiplierAnim = multiplierAnimLow;
            foreach (Animator anim in animators)
            {
                anim.speed = multiplierAnim;
            }
        }
        else if ((isMoving || !isGrounded) && multiplierAnim != multiplierAnimMid)
        {
            multiplierAnim = multiplierAnimMid;
            foreach (Animator anim in animators)
            {
                anim.speed = multiplierAnim;
            }
        }
    }

    public void Dash(Vector3 dir)
    {
        if (!isGrounded)
            return;

        forwardCam = camTransform.forward;
        jump1.Play();
        jump2.Play();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * dashForce, ForceMode.Impulse);
        useGravity = false;
        isDashing = true;
        StartCoroutine(nameof(ResetUseGravity), flyDelay);
        playerTransform.SetParent(null);
    }

    public void LosePowers()
    {
        hasPowers = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isDashing = false;
            ContactPoint contactPoint = collision.GetContact(0);
            gravityDir = -contactPoint.normal;
            rb.velocity = Vector3.zero;
            playerTransform.position = contactPoint.point - contactPoint.normal * feet.transform.localPosition.y;
            playerTransform.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(contactPoint.normal, forwardCam, 90f * Mathf.Deg2Rad, 1f),
                contactPoint.normal);

            playerTransform.SetParent(collision.transform);
            Vector3 parentScale = collision.transform.localScale;
            playerTransform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);

            forwardCam = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        gravityDir = -collision.GetContact(0).normal;
    }

    private IEnumerator ResetUseGravity(float delay)
    {
        yield return new WaitForSeconds(delay);
        useGravity = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feet.transform.position, .5f);
    }
}
