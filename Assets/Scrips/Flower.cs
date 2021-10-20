using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Enermy
{
    float max = 3, min = 0;
    private void Awake()
    {
        max += transform.position.y;
        min += transform.position.y;
    }
    public override void Dead()
    {
        DataManager.instance.ShowScore(100, transform.position.x, transform.position.y + 1);
        Destroy(gameObject);
    }

    public override void Jump()
    {
        
    }

    public override void MoveDown()
    {
        rigid.velocity = new Vector2(0, -3);
        if (transform.position.y <= min)
        {
            rigid.velocity = new Vector2(0, 0);
        }
    }

    public override void MoveLeft()
    {
        
    }

    public override void MoveRight()
    {
        
    }

    public override void MoveUp()
    {
        rigid.velocity = new Vector2(0, 3);
        if (transform.position.y >= max)
        {
            rigid.velocity = new Vector2(0, 0);
        }
    }

    public override void Stand()
    {
        
    }

    public override void Attack()
    {
        
    }

    public override void Dead2()
    {
        DataManager.instance.ShowScore(100, transform.position.x, transform.position.y + 1);
        Destroy(gameObject);
    }
}
