using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public MainCharacter mainCharacter;
    public abstract void Function();
    public abstract void MoveLeft();
    public abstract void MoveRight();
    public abstract void MoveUp();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mainCharacter = collision.collider.GetComponent<MainCharacter>();
        if (mainCharacter)
        {
            Destroy(gameObject);
        }
    }
}

