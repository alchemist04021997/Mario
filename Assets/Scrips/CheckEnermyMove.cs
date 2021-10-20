using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnermyMove : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BotMushroomDwarf mushroomDwarf = collision.GetComponent<BotMushroomDwarf>();
        BotTurtle turtle = collision.GetComponent<BotTurtle>();
        BotFlower flower = collision.GetComponent<BotFlower>();
        if (mushroomDwarf)
        {
            mushroomDwarf.enabled = true;
        }
        else if (turtle)
        {
            turtle.enabled = true;
        }
        else if (flower)
        {
            flower.enabled = true;
        }
    }
}
