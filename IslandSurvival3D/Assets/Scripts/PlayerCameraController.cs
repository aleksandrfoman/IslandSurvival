using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private Transform cameraHolder;
    [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private Transform cameraPosistion;
    private float xRotation;
    private float yRotation;

    private void Update()
    {
        cameraHolder.position = cameraPosistion.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
