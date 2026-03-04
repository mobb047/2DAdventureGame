using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 target;//目标点
    private Vector3 moveDir;//移动方向
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();//每次target都会随机获得一个新的点

    
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if(Mathf.Abs(currentEnemy.transform.position.x - target.x) < 0.1f &&Mathf.Abs(currentEnemy.transform.position.y - target.y) < 0.1f)
        {
            currentEnemy.wait=true;//到达指定位置了，则进入等待时间
            target = currentEnemy.GetNewPoint();//进入等待时间后，target要重新获取一个新的目标点
        }

        //写移动方向
        moveDir = (target - currentEnemy.transform.position).normalized;//目标点和当前位置的差值就是移动方向，normalized是把这个向量变成单位向量

        if (moveDir.x > 0)//面朝方向，不用考虑y轴，因为蜜蜂在天上飞
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);//默认1的时候蜜蜂面朝左侧

        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);

        }
        
    }
    public override void PhysicsUpdate()
    {
        if(!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed*Time.deltaTime;//移动方向乘以速度就是速度矢量
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;//等待的时候，速度为0
        }
            
    }
        public override void OnExit()
    {
        // enemy.anim.SetBool("isPatrol", false);
    }
}
