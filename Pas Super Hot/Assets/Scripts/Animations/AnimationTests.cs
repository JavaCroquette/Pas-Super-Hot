using UnityEngine;

public class AnimationTests : MonoBehaviour
{
    [SerializeField] private Player player;
    private Animator m_Animator;
    private float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animator.SetFloat("Speed", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            Debug.Log(-player.multiplierAnim);
            m_Animator.SetFloat("Speed", -player.multiplierAnim);
        }
        timer -= Time.deltaTime;
    }
}
