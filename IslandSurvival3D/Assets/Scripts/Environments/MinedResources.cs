using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinedResources : MonoBehaviour ,IMineable
{
    [SerializeField]
    protected Item item;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    public Item Mine(Weapon weapon)
    {
        currentHealth -= weapon.Damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        item.SetAmount(weapon.Damage);
        return item;
    }
}
