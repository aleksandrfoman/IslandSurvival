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
    [SerializeField]
    private PlayerInventory playerInventory;
    private void Update()
    {
        Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hitDist);
        if (hit.transform != null)
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.TryGetComponent(out SmallStone smallStone))
            {
                if (smallStone != null)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerInventory.AddItem(smallStone.TakeItem());
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward * hitDist);
        Gizmos.color = Color.red;
    }
}
