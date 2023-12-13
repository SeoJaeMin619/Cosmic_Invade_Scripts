using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SoundManager;

public class TopDownPlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform weaponPivot;
    public static bool isCameraFollowing = true;
    

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;
    Animator animator;

    public static bool isWalking = false;

    void Start()
    {
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent <SpriteRenderer>();
        animator = GetComponent <Animator>();
    }

    void Update()
    {
        if (isCameraFollowing)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveX, moveY).normalized;

            if (movement.magnitude > 0)
            {
                // 플레이어가 이동 중인 경우에만 방향을 설정
                bool isFacingLeft = movement.x < 0;
                spriteRenderer.flipX = isFacingLeft;           
            
            }

            rigidbody2D.velocity = movement * moveSpeed;

            if (Mathf.Abs(rigidbody2D.velocity.x) > 0.3 || Mathf.Abs(rigidbody2D.velocity.y) > 0.3)
            {
                animator.SetBool("IsWalk", true);
                isWalking = true;
                SoundManager.instance.LoopSFXPlay(SFXType.walk);
            }
            else
            {
                animator.SetBool("IsWalk", false);
                isWalking = false;
                SoundManager.ExistWalk = false;
            }
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
            animator.SetBool("IsWalk", false);
            isWalking = false;
            SoundManager.ExistWalk = false;
        }
        UpdateWeaponPivotPosition();
    }
    void UpdateWeaponPivotPosition()
    {
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            weaponPivot.localPosition = new Vector3(0, 3f, 0);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            weaponPivot.localPosition = new Vector3(-3, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            weaponPivot.localPosition = new Vector3(0, -3f, 0);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            weaponPivot.localPosition = new Vector3(3, 0, 0);
        }
    }


}