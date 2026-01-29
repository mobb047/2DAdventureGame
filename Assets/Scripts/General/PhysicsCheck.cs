using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;
    [Header("检测参数")]
    public bool manual;
    public Vector2 bottomOffset;//脚底位移差值
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2 ((coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2);
            leftOffset = new Vector2 (-(coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2);
        }
    }


    private void Update()//每帧都检测;
    {
        Check();
    }
    public void Check()
    {
        //检测地面;
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);//脚底白圈辅助线
        //绘制辅助圆圈线
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }
}
