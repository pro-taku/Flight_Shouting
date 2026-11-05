using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    float time;

    Rigidbody2D rg2d;
    GameObject player;

    void Start()
    {
        time = 0;
        rg2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        FireBullet();
    }

    void Update()
    {
        RotateBullet();
        DestroyBullet();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("BoomMissile"))
        {
            Destroy(gameObject);
        }
    }

    void FireBullet()
    {
        Vector3 distance = player.transform.localPosition - transform.localPosition;
        distance = distance.normalized;
        rg2d.linearVelocity = distance * moveSpeed;
    }

    void RotateBullet()
    {
        transform.rotation = Quaternion.Euler(0, 0, time * rotateSpeed);
    }

    void DestroyBullet()
    {
        time += Time.deltaTime;
        if (time > 5f)
        {
            Destroy(gameObject);
        }
    }
}
