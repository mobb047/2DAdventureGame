using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;//protected表示只有子类可以访问父类的这个变量，且外部不可用

    PhysicsCheck physicsCheck;

   [Header("基本参数")]
   public float normalSpeed;
   public float fastSpeed;  
    public float currentSpeed;
   public Vector3 faceDir;
   public float hurtForce;

   public Transform attacker;

   [Header("计时器")]
   public float waitTime;
   public float waitTimeCounter;
   public bool wait;

   [Header("状态")]
   public bool isHurt;
   public bool isDead;

   private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;    
        
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        if((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        {
            wait = true;
            anim.SetBool("walk", false);
            
        }

        TimeCounter();
        
    }

    private void FixedUpdate()
    {
        if(!isHurt & !isDead)//不是受伤也不是死亡就可以移动了
            Move();
    }

    public virtual void Move()//vitual修饰符,表示子类可以重写父类的函数
    {
        rb.velocity = new Vector2(currentSpeed*faceDir.x*Time.deltaTime, rb.velocity.y);
    }

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
    }

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


}
