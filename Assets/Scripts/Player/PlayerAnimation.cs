using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
   private Animator anim;
   private Rigidbody2D rb;

   private void Awake()
   {
       anim = GetComponent<Animator>();//通过anim访问Animator中变量的内容
       rb = GetComponent<Rigidbody2D>();
   }

    public void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));//取绝对值，因为右跑是正值，左跑是负数，而我们在idle切换run的条件里给的condition是大于0.1
    }
}
