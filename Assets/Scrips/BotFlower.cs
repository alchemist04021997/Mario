using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFlower : MonoBehaviour
{
    public Flower flower;
    float startTime;
    bool isUp;
    private void Awake()
    {
        flower = GetComponent<Flower>();
        startTime = Time.time;
    }
    private void FixedUpdate()
    {
        if ((Time.time - startTime) >= 5)
        {
            isUp = !isUp;
            startTime = Time.time;
        }
        if (isUp)
        {
            flower.MoveUp();
        }
        else
        {
            flower.MoveDown();
        }
    }
}
