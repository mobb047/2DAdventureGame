using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 假设基类中 OnEnter 接收 Enemy 类型参数
public class BeeChaseState : BaseState
{
    // 修复：补全 Enemy 参数，匹配基类抽象方法签名
    public override void OnEnter(Enemy enemy)
    {
        // 可添加进入该状态的初始化逻辑（如设置目标、播放动画）
    }

    // 新增：实现未完成的 OnExit 方法
    public override void OnExit()
    {
        // 可添加退出该状态的清理逻辑（如重置参数、停止音效）
    }

    // 新增：实现未完成的 PhysicsUpdate 方法
    public override void PhysicsUpdate()
    {
        // 可添加物理逻辑（如移动、碰撞检测）
    }

    // 新增：实现未完成的 LogicUpdate 方法
    public override void LogicUpdate()
    {
        // 可添加逻辑更新（如状态切换判断）
    }
}