using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("检测参数")]
    public Vector2 bottomOffset;//脚底位移差值
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGround;

    private void Update()//每帧都检测;
    {
        Check();
    }
    public void Check()
    {
        //检测地面;
        isGround = Physics2D.OverlapCircle((Vector2)transform.position, checkRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);//脚底白圈辅助线
    }
}
