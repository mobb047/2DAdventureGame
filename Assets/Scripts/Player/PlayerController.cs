using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Rigidbody2D rb;//这个rb是刚体
    private CapsuleCollider2D coll;
    private PhysicsCheck physicsCheck;
    public PlayerAnimation playerAnimation;
    public Vector2 inputDirection;
    [Header("基本参数")]
    public float speed;
    public float jumpForce;

    public float hurtForce;

    [Header("状态")]
    
    public bool isHurt;

    public bool isDead;
    public bool isAttack;


    //一些生命周期函数，我也不太清楚叫啥。awake是最先的对象创建时候，每个对象只执行一次，
    //start是第二个的,在创建对象时候，每个对象只执行一次，
    // onenable是在对象启用时候，每次启用都会执行，
    // ondisable是在对象禁用时候，每次禁用都会执行，
    // ondestroy是最后一个对象销毁时候，每个对象只执行一次，
    // update是每帧调用的，
    // fixedupdate是每固定帧调用的，
    // lateupdate是每帧最后调用的
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        //coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        inputControl = new PlayerInputControl();
        //跳跃
        inputControl.GamePlay.Jump.started += Jump;
        //按住用perform，按一下用started,
        // +=表示添加事件/注册,jump添加到按下的那一刻，也意味着这一刻可以添加多个函数

        //攻击
        inputControl.GamePlay.Attack.started +=PlayerAttack;

    }

    

    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()//输入处理
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()//物理移动，键盘wasd移动
    {
        if(!isHurt)
            Move();//如果人物没有在受伤害的状态则可以移动，受伤时不可移动
    }

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;//这里在float类型前面加上（int）表示强制转换成int类型

        if (inputDirection.x < 0)
            faceDir = -1;
        else if (inputDirection.x > 0)
            faceDir = 1;
        //下面是人物翻转,大写的是类型，小写的是变量，这里的transform就是commponent里的transform，
        // 要修改的是transform.localScale，localscale是一个三维向量，有xyz，向左-1，向右1，
        // 在键盘输入时确实是-1和1，但是如果是手柄输入的话，可能是0.5之类的，这里在上面确定一个新变量faceDir，
        // 然后根据输入方向来确定这个变量的值，然后根据这个变量的值来修改transform.localScale,
        // 当输入的的x值小于0，就改成-1，大于0，就是1
        transform.localScale = new Vector3(faceDir, 1, 1);//y and z are the same=1，
        // 什么时候可以确定使用localscale的方法呢？人物图片锚点的位置决定人物的反转方向，锚点在人物中间才可以用。
        // 另外，当scale默认是1，1，1时候，如果手动改变了人物的比例比如改为2，代码中的1就要改为2，faceDir和y,z都要变为2，
        // 还有sprite组件，可以用sprite renderer.flipx
    }
      private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");运行到这里在console会打印Jump
        if(physicsCheck.isGround)//只有人物在地面上时才可以跳跃
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);//世界坐标向上的力，impulse一个瞬时的力
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
       playerAnimation.PlayAttack();
       isAttack = true;
    }

    #region UnityEvent
    //标注
    public void GetHurt(Transform attacker)
    {
        isHurt = true;//伤害状态
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//计算一个从攻击者指向自己的方向向量，并归一化

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);//添加一个瞬时的力，把人物反弹了
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
    #endregion


}
