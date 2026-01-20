using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    override public void Move()
    {
        base.Move();//表示虽然重写，但是依然调用会父类的函数
        anim.SetBool("walk", true);
    }
    
}
