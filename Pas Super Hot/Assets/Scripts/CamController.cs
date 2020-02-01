using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Player player = default;
    [SerializeField] private Vector3 offset = default;
    private float rotationSpeed = 50f;

    private void Awake()
    {
        rotationSpeed = player.rotationSpeed;
    }
    private float xRotation = 0f;

    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            player.Dash(camTransform.forward);
        }
    }
    private void LateUpdate()
    {
        camTransform.localPosition = offset;

        xRotation = camTransform.localRotation.eulerAngles.x;
        if (xRotation >= 270f)
            xRotation -= 360f;
        camTransform.localRotation = Quaternion.Euler(Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, -90f, 90f),
        0f,
        0f);
    }
}
