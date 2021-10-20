using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTurtle : MonoBehaviour
{
    public Turtle character;
    bool isMoveLeft;
    public float startTimeBack;
    Animator animator;
    private void Awake()
    {
        character = GetComponent<Turtle>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (!character.isDead)
        {
            if (isMoveLeft)
            {
                character.MoveLeft();
            }
            else
            {
                character.MoveRight();
            }
        }
        if (character.isDead)
        {
            if ((Time.time - startTimeBack) > 5)
            {
                animator.SetTrigger("Back");
                character.isDead = false;
                animator.SetTrigger("Walk");
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoveLeft = !isMoveLeft;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (character.isDead)
        {
            startTimeBack = Time.time;
        }
    }
}
