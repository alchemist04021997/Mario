using UnityEngine;

public class BulletMain : Bullet
{
    [HideInInspector] public Vector3 velocity;
    private void Awake()
    {
        velocity.x = 0.1f;
    }
    public override void UpdatePosition()
    {
        velocity.y -= 0.01f;
        transform.position += velocity;
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.DrawLine(collision.bounds.ClosestPoint(transform.position), collision.bounds.ClosestPoint(transform.position) + Vector3.up, Color.red, 10);
        if (transform.position.x == collision.bounds.ClosestPoint(transform.position).x)
        {
            velocity.y = -velocity.y;
            transform.position += velocity;
        }
        else
        {
            if (collision.name != "CheckEnermyMove")
            {
                Destroy(gameObject);
            }
        }
        Enermy enermy = collision.GetComponentInChildren<Enermy>();
        if (enermy) 
        {
            enermy.Dead2();
            DataManager.instance.UpdateScore(1000);
            if (transform.position.x < enermy.transform.position.x)
            {
                enermy.rigid.velocity = new Vector2(1, 1);
            }
            else
            {
                enermy.rigid.velocity = new Vector2(-1, 1);
            }
            Destroy(gameObject);
        }
    }
}
