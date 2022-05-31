using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private int hitDist;

    private void Update()
    {

        Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hitDist);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }
}
