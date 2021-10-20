using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMushroomDwarf : MonoBehaviour
{
    Character character;
    bool isMoveLeft;
    private void Awake()
    {
        character = GetComponent<Character>();
    }
    private void FixedUpdate()
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoveLeft = !isMoveLeft;
    }
}
