using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    private void Start()
    {
        currentHealth = maxHealth;//每次新开始游戏当前血量都是满的
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if(invulnerable)
        {
            return;
        }//执行到这里结束这个函数，剩余的伤害被return掉了
        if(currentHealth - attacker.damage > 0){
        currentHealth -= attacker.damage;
        TriggerInvulnerable();//触发一次伤害就执行一次伤害无敌，这样触碰的时候只有一次伤害
        }
        else
        {
            currentHealth = 0;
            //Die();
        }
    }
    /// <summary>
    /// 触发伤害无敌
    /// </summary>

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
