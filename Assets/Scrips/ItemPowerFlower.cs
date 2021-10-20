using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerFlower : Item
{
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public override void Function()
    {
        DataManager.instance.UpdateScore(1000);
        DataManager.instance.ShowScore(1000, transform.position.x, transform.position.y + 1);
    }

    public override void MoveLeft()
    {
        
    }

    public override void MoveRight()
    {
        
    }

    public override void MoveUp()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 1);
    }
}
