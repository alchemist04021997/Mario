using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderEnermy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.collider.GetComponent<Character>();
        if (character)
        {
            GetComponentInParent<Enermy>().Dead();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletMain bullet = collision.GetComponent<BulletMain>();
        if (bullet)
        {
            GetComponentInParent<Enermy>().Dead2();
            if (bullet.transform.position.x < transform.position.x)
            {
                GetComponentInParent<Enermy>().rigid.velocity = new Vector2(1, 1);
            }
            else
            {
                GetComponentInParent<Enermy>().rigid.velocity = new Vector2(-1, 1);
            }
        }
    }
}