using UnityEngine;

public class AnimationTests : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        m_Animator.SetFloat("Speed", move);
    }
}
