using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatform : MonoBehaviour
{
    [SerializeField] Transform platformTransform = default;
    [SerializeField] Material material = default;
    [SerializeField] Color color1 = default;
    [SerializeField] Color color2 = default;
    [SerializeField] float timeAnim = 1f;
    [SerializeField] Transform arrow = default;

    private bool hasWin = false;
    private bool timeDir = true;
    private float timer = 0f;

    private void Awake()
    {
        GetComponent<Renderer>().material = material;
    }
    private void Update()
    {
        if (timeDir)
        {
            timer += Time.deltaTime;
            if (timer > timeAnim)
                timeDir = false;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
                timeDir = true;
        }
        material.color = Color.Lerp(color1, color2, timer);

        arrow.rotation = Quaternion.Euler( Quaternion.LookRotation(platformTransform.position - arrow.position).eulerAngles + Vector3.right * 90f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !hasWin)
        {
            hasWin = true;
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("Vous avez gagné !");
    }
}
