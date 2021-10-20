using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enermy
{
    [HideInInspector] public bool isDead;
    MainCharacter mainCharacter;
    Vector3 space, isDeadMove;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D[] box;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void Dead()
    {
        isDead = true;
        animator.SetTrigger("Head-Down");
        BotTurtle botTurtle = GetComponent<BotTurtle>();
        botTurtle.startTimeBack = Time.time;
        if (mainCharacter)
        {
            space = mainCharacter.transform.position - transform.position;
            if (isDead)
            {
                if (space.x <= 0)
                {
                    isDeadMove.x = 0.2f;
                }
                else
                {
                    isDeadMove.x = -0.2f;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        transform.position += isDeadMove;
    }
    public override void Jump()
    {
        
    }

    public override void MoveDown()
    {
        
    }

    public override void MoveLeft()
    {
        rigid.velocity = new Vector2(-3, rigid.velocity.y);
        spriteRenderer.flipX = false;
    }

    public override void MoveRight()
    {
        rigid.velocity = new Vector2(3, rigid.velocity.y);
        spriteRenderer.flipX = true;
    }

    public override void MoveUp()
    {
        
    }

    public override void Stand()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mainCharacter = collision.collider.GetComponent<MainCharacter>();
        Enermy enermy = collision.collider.GetComponent<Enermy>();
        if (isDead)
        {
            if (enermy)
            {
                enermy.Dead2();
                if (enermy.transform.position.x > transform.position.x)
                {
                    rigid.velocity = new Vector2(3, 3);
                }
                else
                {
                    rigid.velocity = new Vector2(-3, 3);
                }
            }
            else
            {
                isDeadMove.x = -isDeadMove.x;
            }
        }
    }
    public override void Attack()
    {
        
    }
    public override void Dead2()
    {
        isDead = true;
        animator.SetTrigger("Head-Down");
        DataManager.instance.ShowScore(100, transform.position.x, transform.position.y + 1);
        spriteRenderer.flipY = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        box = GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D boxCollider in box)
        {
            boxCollider.enabled = false;
        }
        Destroy(gameObject, 5);
    }
}
