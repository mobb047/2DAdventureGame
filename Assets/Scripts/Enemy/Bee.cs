using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee :Enemy
{
    [Header("移动范围（蜜蜂特有参数绿圈）")]
    public float patrolRadius;

    override protected void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
    }
    
    public override bool FoundPlayer()//重写父类的FoundPlayer方法，改成用圆形范围检测
  {
    var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);//用圆的范围
    if (obj)//如果检测到玩家
    {
      attacker = obj.transform;
    }
    return obj;
  }
    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);
        return spwanPoint + new Vector3(targetX, targetY);//在初始位置的基础上加随机偏移值
    }

    public override void Move()//原来的方法只能横向移动，现在要重写
    {
       //为空，则覆盖掉原来的方法
    }
}
