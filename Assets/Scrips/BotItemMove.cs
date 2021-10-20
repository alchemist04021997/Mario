using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotItemMove : MonoBehaviour
{
    Item item;
    bool isMoveRight = true;
    [HideInInspector] public bool isMoveUp = true;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Block block;
    private void Awake()
    {
        item = GetComponent<Item>();
        startPos = transform.position;
    }
    private void Start()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), block.GetComponent<BoxCollider2D>(), true);
    }
    private void FixedUpdate()
    {
        if ((transform.position.y - startPos.y) >= .7f)
        {
            isMoveUp = false;
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), block.GetComponent<BoxCollider2D>(), false);
        }
        if (isMoveUp)
        {
            item.MoveUp();
        }
        else
        {
            if (isMoveRight)
            {
                item.MoveRight();
            }
            else
            {
                item.MoveLeft();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.position.y <= collision.transform.position.y)
        {
            isMoveRight = !isMoveRight;
        }
    }
}
