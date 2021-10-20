using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDwarf : Enermy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public bool deadByCrushed;
    BotMushroomDwarf botMushroomDwarf;
    BoxCollider2D[] box;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        botMushroomDwarf = GetComponent<BotMushroomDwarf>();
    }
    public override void Dead()
    {
        animator.SetTrigger("Dead");
        DataManager.instance.ShowScore(100, transform.position.x, transform.position.y + 1);
        botMushroomDwarf.enabled = false;
        Destroy(gameObject, 0.2f);
    }

    public override void Jump()
    {
        
    }

    public override void MoveDown()
    {
        
    }

    public override void MoveLeft()
    {
        rigid.velocity = new Vector2(-3, 0);
    }

    public override void MoveRight()
    {
        rigid.velocity = new Vector2(3, 0);
    }

    public override void MoveUp()
    {
        
    }

    public override void Stand()
    {
        
    }

    public override void Attack()
    {
        
    }

    public override void Dead2()
    {
        animator.enabled = false;
        DataManager.instance.ShowScore(100, transform.position.x, transform.position.y + 1);
        spriteRenderer.flipY = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        box = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D boxCollider in box)
        {
            boxCollider.enabled = false;
        }
        Destroy(gameObject, 5);
    }
}
