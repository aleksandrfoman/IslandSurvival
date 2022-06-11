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
    [SerializeField]
    private PlayerMovement playerMovement;
    [Header("HandsController")]
    [SerializeField]
    private Weapon[] weapons;
    private Weapon currentWeapon;
    [SerializeField]
    private float curAttackTime = 0f;
    private void Update()
    {
        ForwardRaycaster();
    }
    public void ForwardRaycaster()
    {
        Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hitDist);

        if (currentWeapon != null)
        {
            if (playerMovement.MoveDirection != Vector3.zero)
            {
                currentWeapon.Animator.SetBool("IsRun", true);
            }
            else
            {
                currentWeapon.Animator.SetBool("IsRun", false);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && curAttackTime>= currentWeapon.ReloadTime)
            {
                currentWeapon.Animator.SetTrigger("Attack");
                curAttackTime = 0f;
                Debug.Log("Attack");
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out IMineable mineable))
                    {
                        playerInventory.AddItem(mineable.Mine(currentWeapon));
                    }
                    //else if()
                }
            }
            else
            {
                curAttackTime +=Time.deltaTime;
            }
        }
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.TryGetComponent(out ITakeable takable))
            {
                if (takable != null)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerInventory.AddItem(takable.TakeItem());
                    }
                }
            }
        }
    }

    public void ActivateWeapon(Item item)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (item != null)
            {
                if (weapons[i].IdName == item.IdName)
                {
                    weapons[i].gameObject.SetActive(true);
                    currentWeapon = weapons[i];
                    Debug.Log("ActivateHands :" + weapons[i].IdName);
                }
                else
                {
                    currentWeapon = null;
                    weapons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                currentWeapon = null;
                weapons[i].gameObject.SetActive(false);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward * hitDist);
        Gizmos.color = Color.red;
    }
}
