using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGazeController : MonoBehaviour
{
    private Transform camTransform;
    private Player myPlayer;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private float rotationSpeed;
    private float xRotation = 0f;

    private void Awake()
    {
        camTransform = Camera.main.GetComponent<Transform>();
        myPlayer = Player.Instance;
        rotationSpeed = myPlayer.rotationSpeed;
    }
    private void Start()
    {
        ApplyOffsetOnCamera();
    }

    private void ApplyOffsetOnCamera()
    {
        camTransform.localPosition = offset;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            myPlayer.Dash(camTransform.forward);
        }
    }
    private void LateUpdate()
    {

        myPlayer.MyTransform.Rotate(
            0f, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0f);
        xRotation = camTransform.localRotation.eulerAngles.x;
        Debug.Log(xRotation);
        if (xRotation >= 270f)
            xRotation -= 360f;
        camTransform.localRotation = Quaternion.Euler(
            Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime,
            -90f, 90f),
        camTransform.localRotation.eulerAngles.y,
        camTransform.localRotation.eulerAngles.z);
    }
}
