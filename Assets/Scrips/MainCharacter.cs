using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : Character
{
    [SerializeField] RuntimeAnimatorController tinyController, bigController, superController;

    [HideInInspector] public Block blockJustHit, anyBlock;
    int countTounchGround;
    [HideInInspector] public bool isTiny = true;
    [HideInInspector] public bool isSuper, isGhost;
    [HideInInspector] public bool isSitting = false;
    bool _isSwiming;
    float countdownTime = 200;
    public AudioClip fireBall, tinyJump, bigJump, die, tinyToBig, bigToTiny, warning;
    bool warningTime = true;

    public bool isSwiming
    {
        get
        {
            return _isSwiming;
        }
    }
    public bool isTouchGround
    {
        get
        {
            return countTounchGround > 0;
        }
    }

    public enum AnimationState
    {
        stand,
        walkLeft,
        walkRight,
        jump,
        dead,
        tinyToBig,
        bigToTiny,
        bigToSuper,
        shoot,
        sit
    }
    string lastNameAnimation;
    AnimationState animationState;
    Animator animator;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public BoxCollider2D boxCollider;
    MainCharacterController mainCharacterController;
    CameraFollow cameraFollow;
    ItemRedMushroom itemRedMushroom;
    ItemPowerFlower itemPowerFlower;
    ItemGreenMushroom itemGreenMushroom;
    Enermy enermy;
    ItemStar itemStar;
    Flag flag;
    [HideInInspector] public bool isImmortal;
    float startTimeImmortal,startTimeGhost;
    Castle castle;
    BoxCollider2D[] enermyCol;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainCharacterController = GetComponent<MainCharacterController>();
        cameraFollow = GetComponent<CameraFollow>();
    }
    private void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
        }
        else
        {
            Dead();
        }
        if (countdownTime < 50)
        {
            if (warningTime)
            {
                SoundManarger.instance.ChangerMusicSource(warning);
                warningTime = false;
            }
        }
        DataManager.instance.CDTime((int)countdownTime);
    }
    public override void Dead()
    {
        ChangeAnimation(AnimationState.dead);
        mainCharacterController.enabled = false;
        rigid.velocity = new Vector2(0, 20);
        GetComponent<Collider2D>().enabled = false;
        DataManager.instance.turnNumber--;
        SoundManarger.instance.PlaySingle(die);
        SoundManarger.instance.musicSource.mute = true;
        Invoke("LoadScence", 2);
    }
    void LoadScence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }
    public override void Jump()
    {
        rigid.velocity = new Vector2(0, 5);
        StartCoroutine(CoroutineWaitUntilJumpOutGround());
        if (isTiny)
        {
            SoundManarger.instance.PlaySingle(tinyJump);
        }
        else
        {
            SoundManarger.instance.PlaySingle(bigJump);
        }
    }
    IEnumerator CoroutineWaitUntilJumpOutGround()
    {
        yield return new WaitUntil(() => !isTouchGround);
        ChangeAnimation(AnimationState.jump);
    }
    public override void MoveLeft()
    {
        rigid.velocity = new Vector2(-3, rigid.velocity.y);
        if (isTouchGround && !isSwiming)
        {
            ChangeAnimation(AnimationState.walkLeft);
        }
        else
        {
            ChangeAnimation(AnimationState.jump);
        }
    }

    public override void MoveRight()
    {
        rigid.velocity = new Vector2(3, rigid.velocity.y);
        if (isTouchGround && !isSwiming)
        {
            ChangeAnimation(AnimationState.walkRight);
        }
        else
        {
            ChangeAnimation(AnimationState.jump);
        }
    }

    public override void MoveUp()
    {
        if (isSwiming)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 5);
        }
    }

    public override void MoveDown()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, -5);
    }
    public void Sit()
    {
        if (isTouchGround)
        {
            if (!isTiny)
            {
                isSitting = true;
                ChangeAnimation(AnimationState.sit);
            }
        }
    }
    public override void Stand()
    {
        if (isTouchGround)
        {
            rigid.velocity = new Vector2(rigid.velocity.x * 0.5f, rigid.velocity.y);
            ChangeAnimation(AnimationState.stand);
        }
    }
    private void LateUpdate()
    {
        CheckTimeImmortal();
        CheckTimeGhost();
        if (blockJustHit)
        {
            blockJustHit.Bounce();
            blockJustHit = null;
        }
    }

    private void CheckTimeGhost()
    {
        if (isGhost)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            if ((Time.time - startTimeGhost) >= 2)
            {
                isGhost = false;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                foreach (BoxCollider2D collider in enermyCol)
                {
                    Physics2D.IgnoreCollision(boxCollider, collider, false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        castle = collision.collider.GetComponent<Castle>();
        enermyCol = collision.collider.GetComponentsInChildren<BoxCollider2D>();
        enermy = collision.collider.GetComponent<Enermy>();
        itemRedMushroom = collision.collider.GetComponent<ItemRedMushroom>();
        itemGreenMushroom = collision.collider.GetComponent<ItemGreenMushroom>();
        itemPowerFlower = collision.collider.GetComponent<ItemPowerFlower>();
        anyBlock = collision.collider.GetComponent<Block>();
        itemStar = collision.collider.GetComponent<ItemStar>();
        flag = collision.collider.GetComponent<Flag>();


        countTounchGround++;
        if (enermy)
        {
            if (isTiny)
            {
                if (isImmortal)
                {
                    enermy.Dead();
                    DataManager.instance.UpdateScore(100);
                }
                else
                {
                    Dead();
                }
            }
            else
            {
                if (isImmortal)
                {
                    enermy.Dead();
                    DataManager.instance.UpdateScore(100);
                }
                else
                {
                    Ghost();
                }
            }
        }
        if (collision.collider.name == "HeadColliderEnermy")
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 10);
        }
        TinyToBig();
        BigToSuper();
        Immortal();
        if (itemGreenMushroom)
        {
            itemGreenMushroom.Function();
        }
        JumpOnTheFlagpole();
        if (anyBlock)
        {
            mainCharacterController.isKeyJumpDown = false;
        }
    }

    void JumpOnTheFlagpole()
    {
        if (flag)
        {
            if ((transform.position.y - flag.transform.position.y) > 0.9f && (transform.position.y - flag.transform.position.y) < 1.8f)
            {
                flag.ShowScore(100);
            }
            else if ((transform.position.y - flag.transform.position.y) > 1.8f && (transform.position.y - flag.transform.position.y) < 2.7f)
            {
                flag.ShowScore(200);
            }
            else if ((transform.position.y - flag.transform.position.y) > 2.7f && (transform.position.y - flag.transform.position.y) < 3.6f)
            {
                flag.ShowScore(300);
            }
            else if ((transform.position.y - flag.transform.position.y) > 3.6f && (transform.position.y - flag.transform.position.y) < 4.5f)
            {
                flag.ShowScore(400);
            }
            else if ((transform.position.y - flag.transform.position.y) > 4.5f && (transform.position.y - flag.transform.position.y) < 5.4f)
            {
                flag.ShowScore(500);
            }
            else if ((transform.position.y - flag.transform.position.y) > 5.4f && (transform.position.y - flag.transform.position.y) < 6.3f)
            {
                flag.ShowScore(600);
            }
            else if ((transform.position.y - flag.transform.position.y) > 6.3f && (transform.position.y - flag.transform.position.y) < 7.2f)
            {
                flag.ShowScore(700);
            }
            else if ((transform.position.y - flag.transform.position.y) > 7.2f && (transform.position.y - flag.transform.position.y) < 8.1f)
            {
                flag.ShowScore(800);
            }
            else if ((transform.position.y - flag.transform.position.y) > 8.1f && (transform.position.y - flag.transform.position.y) < 9)
            {
                flag.ShowScore(900);
            }
            else if ((transform.position.y - flag.transform.position.y) > 9)
            {
                flag.ShowScore(1000);
            }
            StartCoroutine(CoroutineFinish());
        }
    }
    IEnumerator CoroutineFinish()
    {
        mainCharacterController.enabled = false;
        cameraFollow.enabled = false;
        yield return new WaitForSeconds(1);
        spriteRenderer.flipX = true;
        transform.position = new Vector2(transform.position.x + 0.75f, transform.position.y);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = false;
        while (true)
        {
            yield return null;
            MoveRight();
        }
    }
    void Ghost()
    {
        startTimeGhost = Time.time;
        isGhost = true;
        isTiny = true;
        isSuper = false;
        ChangeAnimation(AnimationState.bigToTiny);
        foreach (BoxCollider2D collider in enermyCol)
        {
            Physics2D.IgnoreCollision(boxCollider, collider, true);
        }
        SoundManarger.instance.PlaySingle(bigToTiny);
    }
    void Immortal()
    {
        if (itemStar)
        {
            isImmortal = true;
            itemStar.Function();
            startTimeImmortal = Time.time;
        }
    }
    void CheckTimeImmortal()
    {
        if (isImmortal)
        {
            if ((Time.time - startTimeImmortal) >= 10)
            {
                isImmortal = false;
            }
        }
    }
    void TinyToBig()
    {
        if (itemRedMushroom)
        {
            if (isTiny)
            {
                isTiny = false;
                ChangeAnimation(AnimationState.tinyToBig);
            }
            itemRedMushroom.Function();
            SoundManarger.instance.PlaySingle(tinyToBig);
        }
    }
    void BigToSuper()
    {
        if (itemPowerFlower)
        {
            if (!isTiny)
            {
                isSuper = true;
                ChangeAnimation(AnimationState.bigToSuper);
            }
            itemPowerFlower.Function();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        countTounchGround--;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        WaterZone waterZone = collision.GetComponent<WaterZone>();
        if (waterZone)
        {
            _isSwiming = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isSwiming = false;
    }

    public override void Attack()
    {
        if (isSuper)
        {
            ChangeAnimation(AnimationState.shoot);
            GameObject redMushroom = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject)));
            if (spriteRenderer.flipX)
            {
                redMushroom.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y + 1);
                redMushroom.GetComponent<BulletMain>().velocity.x = -redMushroom.GetComponent<BulletMain>().velocity.x;
            }
            else
            {
                redMushroom.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 1);
            }
            SoundManarger.instance.PlaySingle(fireBall);
        }
    }

    IEnumerator WaitForSeconds(Action action,float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    float lastChangeAnimation;
    int waitingAnimNumber = 0;
    void ChangeAnimation(AnimationState animation)
    {
        if (animation != animationState)
        {
            waitingAnimNumber++;
            animationState = animation;
            lastChangeAnimation = Time.time;
            Action action = () =>
            {
                waitingAnimNumber--;
                switch (animation)
                {
                    case AnimationState.stand:
                        AnimationStand();
                        break;
                    case AnimationState.jump:
                        AnimationJump();
                        break;
                    case AnimationState.walkLeft:
                        AnimationWalkLeft();
                        break;
                    case AnimationState.walkRight:
                        AnimationWalkRight();
                        break;
                    case AnimationState.dead:
                        AnimationDead();
                        break;
                    case AnimationState.tinyToBig:
                        AnimationTinyToBig();
                        break;
                    case AnimationState.bigToTiny:
                        AnimationBigToTiny();
                        break;
                    case AnimationState.bigToSuper:
                        AnimationBigToSuper();
                        break;
                    case AnimationState.shoot:
                        AnimationShoot();
                        break;
                    case AnimationState.sit:
                        AnimationSit();
                        break;
                }
            };
            if (Time.time - lastChangeAnimation <= Time.deltaTime * waitingAnimNumber)
            {
                StartCoroutine(WaitForSeconds(action, waitingAnimNumber * Time.deltaTime - Time.time + lastChangeAnimation));
            }
            else
            {
                action?.Invoke();
            }
        }
    }

    private void AnimationSit()
    {
        animator.SetInteger("Action", 4);
        boxCollider.offset = new Vector2(0, 0.12f);
        boxCollider.size = new Vector2(0.16f, 0.22f);
    }

    private void AnimationShoot()
    {
        animator.SetInteger("Action", 5);
    }

    private void AnimationBigToSuper()
    {
        animator.SetInteger("Action", 8);
        animator.runtimeAnimatorController = superController;
    }

    private void AnimationBigToTiny()
    {
        animator.SetInteger("Action", 7);
        animator.runtimeAnimatorController = tinyController;
        boxCollider.offset = new Vector2(0, 0.08f);
        boxCollider.size = new Vector2(0.13f, 0.16f);
    }

    private void AnimationTinyToBig()
    {
        animator.SetInteger("Action", 7);
        animator.runtimeAnimatorController = bigController;
        boxCollider.offset = new Vector2(0, 0.16f);
        boxCollider.size = new Vector2(0.16f, 0.32f);
    }

    private void AnimationDead()
    {
        animator.SetInteger("Action", 6);
    }

    private void AnimationWalkRight()
    {
        spriteRenderer.flipX = false;
        animator.SetInteger("Action", 1);
    }

    private void AnimationWalkLeft()
    {
        spriteRenderer.flipX = true;
        animator.SetInteger("Action", 1);
    }

    private void AnimationJump()
    {
        animator.SetInteger("Action", 2);
    }

    private void AnimationStand()
    {
        animator.SetInteger("Action", 0);
    }
}