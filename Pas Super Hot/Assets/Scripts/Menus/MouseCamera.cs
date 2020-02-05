using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private float sensitivity = 200f;
    public Transform player;
    float upDownROtation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float Xmove = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float Ymove = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        upDownROtation -= Ymove;
        upDownROtation = Mathf.Clamp(upDownROtation, -90, 90);
        transform.localRotation = Quaternion.Euler(upDownROtation, 0f, 0f);
        player.Rotate(Vector3.up * Xmove);

    }
}
