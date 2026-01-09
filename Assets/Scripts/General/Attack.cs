using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamage(this);
        //?用来判断碰撞对方身上有没有Character脚本，有的话就执行TakeDamage函数，没有的话就不执行
    }
}
