using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Rigidbody2D rigid;
    public abstract void Dead();
    public abstract void Stand();
    public abstract void MoveLeft();
    public abstract void MoveRight();
    public abstract void MoveUp();
    public abstract void MoveDown();
    public abstract void Jump();
    public abstract void Attack();
}
