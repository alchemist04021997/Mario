using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHideMultipleCoin : Block
{
    Vector3 startPosition;
    [SerializeField] Sprite emptyBlockSprite;
    private void Awake()
    {
        startPosition = transform.position;
    }
    MainCharacter mainCharacter;
    Enermy enermy;
    [SerializeField] int coinNumber;
    bool canBounce = true;

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
    private void OnCollisionStay2D(Collision2D collision)
    {
        enermy = collision.collider.GetComponent<Enermy>();
    }
    public override void Bounce()
    {
        if (canBounce)
        {
            if (coinNumber > 0)
            {
                StartCoroutine(CoroutineBounce());
                PresentCoin();
                KillEnermyBehigh();
                coinNumber--;
            }
            else
            {
                ChangeSprite();
                canBounce = false;
            }
        }
    }
    void ChangeSprite()
    {
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
