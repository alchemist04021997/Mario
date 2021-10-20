using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    MainCharacter character;
    [HideInInspector] public bool isKeyJumpDown;

    private void Awake()
    {
        character = GetComponent<MainCharacter>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3();
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            {
                SoundManarger.instance.PlaySingle(character.fireBall);
                GameObject redMushroom = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject)));
                redMushroom.transform.position = mousePos;
            }
        }
        if (Input.GetKey(KeyCode.A) && !character.isSitting)
        {
            character.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D) && !character.isSitting)
        {
            character.MoveRight();
        }
        else
        {
            if (character.isTouchGround)
            {
                if (!character.isTiny)
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        character.Sit();
                    }
                    else
                    {
                        character.Stand();
                        character.isSitting = false;
                    }
                }
                else
                {
                    character.Stand();
                }
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            character.MoveUp();
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            character.MoveDown();
        }
        if (isKeyJumpDown)
        {
            if(character.rigid.velocity.y <= 13)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    character.rigid.velocity += new Vector2(0, 2);
                }
            }
            else
            {
                isKeyJumpDown = false;
            }
        }
    }
    private void Update()
    {
        if (character.isTouchGround && Input.GetKeyDown(KeyCode.Space) && !character.isSitting)
        {
            character.Jump();
            isKeyJumpDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isKeyJumpDown = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            character.Attack();
        }
        if (Input.GetKeyUp(KeyCode.S) && !character.isTiny)
        {
            character.boxCollider.offset = new Vector2(0, 0.16f);
            character.boxCollider.size = new Vector2(0.16f, 0.32f);
        }
    }
}
