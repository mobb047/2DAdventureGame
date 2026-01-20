using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;//protected表示只有子类可以访问父类的这个变量，且外部不可用

   [Header("基本参数")]
   public float normalSpeed;
   public float fastSpeed;  
    public float currentSpeed;
   public Vector3 faceDir;

   private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = normalSpeed;    
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()//vitual修饰符,表示子类可以重写父类的函数
    {
        rb.velocity = new Vector2(currentSpeed*faceDir.x*Time.deltaTime, rb.velocity.y);
    }
}
