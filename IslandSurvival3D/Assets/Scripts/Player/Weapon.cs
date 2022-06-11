using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private string idName;
    public string IdName => idName;

    [SerializeField]
    private int damage;
    public int Damage => damage;

    [SerializeField]
    private Animator animator;
    public Animator Animator => animator;

    [SerializeField]
    private float reloadTime;
    [SerializeField]
    public float ReloadTime => reloadTime;

}
