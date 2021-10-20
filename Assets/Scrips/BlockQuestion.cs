using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockQuestion : Block
{
    Vector2 startPosition;
    [SerializeField] Sprite emptyBlockSprite;
    MainCharacter mainCharacter;
    Enermy enermy;
    bool canBounce = true;
    public enum HideItem { coin, redMushroom, greenMushroom, powerFlower, star}
    public HideItem hideItem;

    private void Awake()
    {
        startPosition = transform.position;
    }
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
    IEnumerator CoroutineBounce()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 5 * Time.deltaTime);
            if (transform.position.y >= startPosition.y + 0.5f)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 5 * Time.deltaTime);
            if (transform.position.y <= startPosition.y)
            {
                transform.position = startPosition;
                break;
            }
            yield return null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        enermy = collision.collider.GetComponent<Enermy>();
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
    void ChangeSprite()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
    }
    void PresentCoin()
    {
        GameObject spinningCoin = (GameObject)Instantiate(Resources.Load("Prefabs/Coin", typeof(GameObject)));
        spinningCoin.transform.SetParent(this.transform.parent);
        spinningCoin.transform.localPosition = new Vector2(startPosition.x, startPosition.y + 0.35f);
        StartCoroutine(BouncingCoin(spinningCoin));
    }
    IEnumerator BouncingCoin(GameObject coin)
    {
        while (true)
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y + 15 * Time.deltaTime);
            if (coin.transform.localPosition.y >= startPosition.y + 4)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - 15 * Time.deltaTime);
            if (coin.transform.localPosition.y <= startPosition.y + 2)
            {
                DataManager.instance.UpdateScore(200);
                DataManager.instance.UpdateCoin();
                DataManager.instance.ShowScore(200, startPosition.x, startPosition.y + 0.35f);
                Destroy(coin.gameObject);
                break;
            }
            yield return null;
        }
    }
    IEnumerator BounceAndCreateMovingItem()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 5 * Time.deltaTime);
            if (transform.position.y >= startPosition.y + 0.5f)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 5 * Time.deltaTime);
            if (transform.position.y <= startPosition.y)
            {
                transform.position = startPosition;
                if (HideItem.redMushroom == hideItem)
                {
                    GameObject createdItem = (GameObject)Instantiate(Resources.Load("Prefabs/Red Mushroom Item", typeof(GameObject)));
                    createdItem.GetComponent<BotItemMove>().block = this;
                    createdItem.transform.position = new Vector2(startPosition.x, startPosition.y - 0.35f);
                    createdItem.GetComponent<BotItemMove>().startPos = createdItem.transform.position;
                }
                else if (HideItem.greenMushroom == hideItem)
                {
                    GameObject createdItem = (GameObject)Instantiate(Resources.Load("Prefabs/Green Mushroom Item", typeof(GameObject)));
                    createdItem.GetComponent<BotItemMove>().block = this;
                    createdItem.transform.position = new Vector2(startPosition.x, startPosition.y - 0.35f);
                    createdItem.GetComponent<BotItemMove>().startPos = createdItem.transform.position;
                }
                else if (HideItem.powerFlower == hideItem)
                {
                    GameObject createdItem = (GameObject)Instantiate(Resources.Load("Prefabs/Power Flower Item", typeof(GameObject)));
                    createdItem.GetComponent<BotItemMove>().block = this;
                    createdItem.transform.position = new Vector2(startPosition.x, startPosition.y - 0.35f);
                    createdItem.GetComponent<BotItemMove>().startPos = createdItem.transform.position;
                }
                else if (HideItem.star == hideItem)
                {
                    GameObject createdItem = (GameObject)Instantiate(Resources.Load("Prefabs/Star Item", typeof(GameObject)));
                    createdItem.GetComponent<BotItemMove>().block = this;
                    createdItem.transform.position = new Vector2(startPosition.x, startPosition.y - 0.35f);
                    createdItem.GetComponent<BotItemMove>().startPos = createdItem.transform.position;
                }
                break;
            }
            yield return null;
        }
    }

    public override void Bounce()
    {
        if (canBounce)
        {
            switch (hideItem)
            {
                case HideItem.coin:
                    StartCoroutine(CoroutineBounce());
                    PresentCoin();
                    ChangeSprite();
                    KillEnermyBehigh();
                    break;
                case HideItem.redMushroom:
                    StartCoroutine(BounceAndCreateMovingItem());
                    ChangeSprite();
                    KillEnermyBehigh();
                    break;
                case HideItem.greenMushroom:
                    StartCoroutine(BounceAndCreateMovingItem());
                    ChangeSprite();
                    KillEnermyBehigh();
                    break;
                case HideItem.powerFlower:
                    StartCoroutine(BounceAndCreateMovingItem());
                    ChangeSprite();
                    KillEnermyBehigh();
                    break;
                case HideItem.star:
                    StartCoroutine(BounceAndCreateMovingItem());
                    ChangeSprite();
                    KillEnermyBehigh();
                    break;
            }
            canBounce = false;
        }
    }
}
