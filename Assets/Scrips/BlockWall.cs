using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWall : Block
{
    Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
    }
    MainCharacter mainCharacter;
    Enermy enermy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mainCharacter = collision.collider.GetComponent<MainCharacter>();
        if (mainCharacter && collision.collider.bounds.ClosestPoint(transform.position).y <= transform.position.y - 0.3f)
        {
            if (mainCharacter.blockJustHit)
            {
                if (Mathf.Abs(transform.position.x - collision.transform.position.x) < Mathf.Abs(mainCharacter.blockJustHit.transform.position.x - collision.transform.position.x))
                {
                    mainCharacter.blockJustHit = this;
                }
            }
            else
            {
                mainCharacter.blockJustHit = this;
            }
        }
    }

    public override void Bounce()
    {
        if (mainCharacter.isTiny)
        {
            StartCoroutine(CoroutineBounce());
        }
        else
        {
            Destroy(gameObject);
        }
        KillEnermyBehigh();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        enermy = collision.collider.GetComponent<Enermy>();
    }
    IEnumerator CoroutineBounce()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5 * Time.deltaTime, 0);
            if (transform.position.y >= startPosition.y + 0.5f)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 5 * Time.deltaTime, 0);
            if (transform.position.y <= startPosition.y)
            {
                transform.position = startPosition;
                break;
            }
            yield return null;
        }
    }

    void KillEnermyBehigh()
    {
        if (enermy)
        {
            enermy.Dead2();
            if (transform.position.x < enermy.transform.position.x)
            {
                enermy.rigid.velocity = new Vector2(1, 1);
            }
            else
            {
                enermy.rigid.velocity = new Vector2(-1, 1);
            }
        }
    }
}
