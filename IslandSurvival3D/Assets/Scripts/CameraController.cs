using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraState
{
    Menu,
    Player
}
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraState cameraState;
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

    private void Start()
    {
        SetCameraState(cameraState);
    }
    private void Update()
    {
        if (cameraState == CameraState.Menu)
        {
            //transform.LookAt(new Vector3(0, 0, 0f));
            transform.RotateAround(Vector3.zero,Vector3.up, cameraSpeed * Time.deltaTime);
        }
        else if (cameraState == CameraState.Player)
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

    public void SetCameraState(CameraState cameraState)
    {
        if (cameraState == CameraState.Menu)
        {

        }
        else if (cameraState == CameraState.Player)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
