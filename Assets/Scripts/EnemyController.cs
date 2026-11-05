using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 정보
    int hp;
    string tagName;
    int score;

    // 총알
    public GameObject enemyBullet;
    float fireDelay;

    // 플레이어
    GameObject player;
    PlayerController playerController;

    // 애니메이션
    Animator animator;
    bool onDead;
    float animeRuntime;

    // 움직임
    Rigidbody2D rg2d;
    float moveSpeed;

    // 아이템
    public GameObject[] items;

    void Start()
    {
        animator = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        onDead = false;
        fireDelay = 2.5f;
        animeRuntime = 0;
        moveSpeed = Random.Range(5f, 7f);
        hp = gameObject.CompareTag("ItemDropEnemy") ? 3 : 1;
        tagName = gameObject.tag;
        score = 10;

        Move();
    }

    void Update()
    {
        if (onDead)
        {
            animeRuntime += Time.deltaTime;
        }
        if (animeRuntime > 0.6f)
        {
            Destroy(gameObject);
            if (tagName == "ItemDropEnemy")
            {
                int index = Random.Range(0, 2);
                Instantiate(items[index], transform.localPosition, Quaternion.identity);
            }
        }
        Firebullet();
    }

    void Move()
    {
        if (player == null) return;

        Vector3 distance = player.transform.localPosition - transform.localPosition;
        Vector3 direction = distance.normalized;
        rg2d.linearVelocity = direction * moveSpeed;
    }

    void Firebullet()
    {
        if (player == null) return;

        fireDelay += Time.deltaTime;
        if (fireDelay > 3.0f)
        {
            Instantiate(enemyBullet, transform.localPosition, Quaternion.identity);
            fireDelay -= 3f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            hp -= playerController.damage;
        }
        if (collision.CompareTag("BoomMissile"))
        {
            hp -= playerController.boomDamage;
        }
        if (collision.CompareTag("BlockCollider"))
        {
           Destroy(gameObject);
        }
        if (hp <= 0) 
        {
            animator.SetInteger("State", 1);
            OnDead();            
        }
    }

    void OnDead()
    {
        onDead = true;
        if (gameObject.tag != "Untagged")
        {
            SoundManagerController.instance.enemyDeadSound.Play();
            UIManagerController.instance.AddScore(score);
        }
        gameObject.tag = "Untagged";
    }
}
