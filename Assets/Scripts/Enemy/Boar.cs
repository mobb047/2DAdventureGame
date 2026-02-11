using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy//父类继承
{
   protected override void Awake()
    {
        base.Awake();//基本的awake都执行了,在这个基础上再添加一些新的东西
        patrolState = new BoarPatrolState();//new了一个野猪巡逻，后面可以new蜜蜂等等其他的敌人
        
    }
}
