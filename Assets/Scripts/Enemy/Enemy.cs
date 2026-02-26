using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]//要求这个物体必须有这三个组件
public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector]public Animator anim;//protected表示只有子类可以访问父类的这个变量，且外部不可用,后面因为有限状态机所以改成public了

    [HideInInspector]public PhysicsCheck physicsCheck;//[HideInInspector]前缀可以隐藏这个参数不在unity中显示

   [Header("基本参数")]
   public float normalSpeed;
   public float chaseSpeed;  
   [HideInInspector] public float currentSpeed;
   public Vector3 faceDir;
   public float hurtForce;

   public Transform attacker;

   [Header("检测参数")]
   public Vector2 centerOffset;
   public Vector2 checkSize;
   public float checkDistance;
   public LayerMask attackLayer;

   [Header("计时器")]
   public float waitTime;
   public float waitTimeCounter;
   public bool wait;
   public float lostTime;
   public float lostTimeCounter;

   [Header("状态")]
   public bool isHurt;
   public bool isDead;
   protected BaseState patrolState;//巡逻状态
   protected BaseState chaseState;//追逐状态
   protected BaseState currentState;//当前状态

   protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;    
        
    }
    private void OnEnable()
    {
        
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        currentState.LogicUpdate();
        TimeCounter();
        
    }

  
    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();

        if(!isHurt && !isDead && !wait){//不是受伤也不是死亡就可以移动了
            Move();
        }

    }
      
    //上面是原有的，下面的fixed是ai改的
  /* 
    private void FixedUpdate()
{
    if(!isHurt && !isDead && !wait && physicsCheck.isGround)
    {
        Move();
    }
    else
    {
        // 停止时也更新动画参数
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetFloat("speed", 0);
    }
    
    currentState.PhysicsUpdate();
}
  */
//fixed改到这里上面

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move()//vitual修饰符,表示子类可以重写父类的函数
    {
        
        if(wait){
            rb.velocity = new Vector2(0, rb.velocity.y); // 完全停止水平移动
            return;}//如果在等待就不移动了
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("snailPreMove"))
        rb.velocity = new Vector2(currentSpeed*faceDir.x*Time.deltaTime, rb.velocity.y);
    }  

//上面是原来的move，下面是ai改的，到summary
/*
public virtual void Move()
{
    rb.velocity = new Vector2(currentSpeed*faceDir.x*Time.deltaTime, rb.velocity.y);
    
    // 控制动画播放
    float speed = Mathf.Abs(rb.velocity.x);
    anim.SetFloat("speed", speed);
    
    // 调试信息
    Debug.Log($"Speed: {speed}, Velocity X: {rb.velocity.x}");
}
*/

    /// <summary>
    /// 计时器
    /// </summary>
public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!FoundPlayer()&&lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if (FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset, checkSize,0,faceDir,checkDistance,attackLayer);
    }//发射一个方形的判断器，检测player的图层

    public void SwitchState(NPCState state)//语法塘
    {
        var newState = state switch 
            {
                NPCState.Patrol => patrolState,
                NPCState.Chase => chaseState,
                _ => null

            };
            currentState.OnExit();//执行上一个状态的退出
            currentState = newState;//切换状态
            newState.OnEnter(this);//执行新状态的进入

    }


#region 事件执行方法

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //转身
        if(attackTrans.position.x > transform.position.x)//站在野猪右侧
            transform.localScale = new Vector3(-1, 1, 1);
        if(attackTrans.position.x < transform.position.x)//站在野猪左侧
            transform.localScale = new Vector3(1, 1, 1);

        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);//先把敌人水平速度置0，保证每次受伤击退的力度一样
        StartCoroutine(OnHurt(dir));//启动携程
        
    }

    private IEnumerator OnHurt(Vector2 dir)//携程，可以按照一定顺序执行,迭代器
    {
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;   //在敌人死亡瞬间不要再跟玩家产生碰撞，死亡瞬间图层改为第二层，在edit-project settings-physics2D-碰撞矩阵layer colliders matrix中第二层和玩家所在的默认层是不产生碰撞的
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);//销毁当前的gameObject
    }

    #endregion


private void OnDrawGizmosSelected()//选中物体时显示检测范围
    {
        Gizmos.DrawWireSphere(transform.position+(Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }

}
