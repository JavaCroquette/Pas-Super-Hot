using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    #endregion

    #region Get/Set

    public Vector3 GravityDir
    {
        get
        {
            return gravityDir;
        }

        set
        {
            gravityDir = value;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        set
        {
            isGrounded = value;
        }
    }

    public bool UseGravity
    {
        get
        {
            return useGravity;
        }

        set
        {
            useGravity = value;
        }
    }

    public bool HasPowers
    {
        get
        {
            return hasPowers;
        }

        set
        {
            hasPowers = value;
        }
    }

    public bool IsDashing
    {
        get
        {
            return isDashing;
        }

        set
        {
            isDashing = value;
        }
    }

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        set
        {
            isMoving = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public float DashForce
    {
        get
        {
            return dashForce;
        }

        set
        {
            dashForce = value;
        }
    }

    public float GravityForce
    {
        get
        {
            return gravityForce;
        }

        set
        {
            gravityForce = value;
        }
    }

    public float FlyDelay
    {
        get
        {
            return flyDelay;
        }

        set
        {
            flyDelay = value;
        }
    }

    public float TurnDistance
    {
        get
        {
            return turnDistance;
        }

        set
        {
            turnDistance = value;
        }
    }

    public Rigidbody MyRigidbody
    {
        get
        {
            return myRigidbody;
        }

        set
        {
            myRigidbody = value;
        }
    }

    public Transform MyTransform
    {
        get
        {
            return myTransform;
        }

        set
        {
            myTransform = value;
        }
    }
    #endregion

    #region Parameters
    [SerializeField]
    private float speed = 200f, dashForce = 30f,
        gravityForce = 1255.68f, flyDelay = 1f, turnDistance = 10f;
    [SerializeField]
    private float multiplierAnimHigh = 0.1f,
        multiplierAnimMid = 0.04f, multiplierAnimLow = 0.01f;
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Transform myTransform;


    [SerializeField] private Transform camTransform = default;
    [SerializeField] private GameObject feet = default;
    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private AudioSource jump1 = default, jump2 = default;
    public float rotationSpeed = 50f, multiplierAnim = 0f;
    private Vector3 gravityDir = Vector3.down, forwardCam = Vector3.zero;
    [SerializeField]
    private bool isGrounded = true,
        useGravity = true,
        hasPowers = true,
        isDashing = false, isMoving = false;

    private Animator[] animators;

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myTransform = gameObject.GetComponent<Transform>();
    }



    private void Update()
    {
        
        isGrounded = Physics.OverlapSphere(feet.transform.position, .5f, groundLayer).Length > 0;
        if (isGrounded && !isDashing)
        {
            Vector3 newVelocity = speed * Time.deltaTime * (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
            isMoving = !newVelocity.Equals(Vector3.zero);
            if (isMoving)
            {
                myRigidbody.velocity = newVelocity;
            }
        }
        if (isDashing)
        {
            if (Physics.Raycast(myTransform.position, forwardCam, out RaycastHit hitinfo, turnDistance))
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                Vector3.RotateTowards(hitinfo.normal, forwardCam, 90f * Mathf.Deg2Rad, 1f),
                hitinfo.normal);

                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, (turnDistance - hitinfo.distance) / turnDistance);
            }
            else
            {
                forwardCam = myRigidbody.velocity.normalized;
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
            //foreach (Animator anim in animators)
            //{
            //    anim.speed = multiplierAnim;
            //}
        }
        else if (!isDashing && !isMoving && multiplierAnim != multiplierAnimLow)
        {
            multiplierAnim = multiplierAnimLow;
            //foreach (Animator anim in animators)
            //{
            //    anim.speed = multiplierAnim;
            //}
        }
        else if ((isMoving || !isGrounded) && multiplierAnim != multiplierAnimMid)
        {
            multiplierAnim = multiplierAnimMid;
            //foreach (Animator anim in animators)
            //{
            //    anim.speed = multiplierAnim;
            //}
        }
    }

    public void Dash(Vector3 dir)
    {
        if (!isGrounded)
            return;

        forwardCam = camTransform.forward;
        jump1.Play();
        jump2.Play();
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.AddForce(dir * dashForce, ForceMode.Impulse);
        useGravity = false;
        isDashing = true;
        StartCoroutine(nameof(ResetUseGravity), flyDelay);
        myTransform.SetParent(null);
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
            GravityDir = -contactPoint.normal;
            myRigidbody.velocity = Vector3.zero;
            myTransform.position = contactPoint.point - contactPoint.normal * feet.transform.localPosition.y;
            myTransform.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(contactPoint.normal, forwardCam, 90f * Mathf.Deg2Rad, 1f),
                contactPoint.normal);

            myTransform.SetParent(collision.transform);
            Vector3 parentScale = collision.transform.localScale;
            myTransform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);

            forwardCam = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        GravityDir = -collision.GetContact(0).normal;
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
