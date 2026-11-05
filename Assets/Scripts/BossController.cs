using System.Collections;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;

    public float hp1;
    public float hp2;

    Animator animator;
    /// <summary>
    /// 애니메이션 상태 확인용
    /// -1 : 대기, 이동 반복
    /// 0 : 대기, 이동
    /// 1 : 죽음
    /// 2 : L 공격
    /// 3 : R 공격
    /// </summary>
    int animeNum;

    bool onDead;
    bool isSpawning;

    int score;
    float time;
    Transform spwanMovePos;
    float speed;

    public Transform LAttackPos;
    public Transform RAttackPos;
    public GameObject bossBullet;
    float fireDelay;

    public SpriteRenderer spriteRenderer;
    Color color;

    void Awake()
    {
        hp2 = 150f;
        hp1 = 150f;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        spwanMovePos = GameObject.Find("BossSpawn").GetComponent<Transform>();

        animator = GetComponent<Animator>();

        onDead = false;
        isSpawning = true;

        score = 1000;

        speed = 10;
        time = 0;
        fireDelay = 0;
        animeNum = 0;

        color = spriteRenderer.color;
    }

    void Update()
    {
        if (isSpawning)
        {
            SpawnBoss();
        }  
        if (onDead)
        {
            time += Time.deltaTime;
        }
        if (time >= 0.6)
        {
            Destroy(gameObject);
        }
        if (player == null && GameManageerController.instance.life >= 0)
        {
            FindPlayer();
        }
        FireBullet();
        AnimationSystem();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (hp1 > 0) hp1 -= playerController.damage;
            else hp2 -= playerController.damage;
            StartCoroutine(onDamagedColor());
        }
        if (collision.CompareTag("BoomMissile"))
        {
            if (hp1 > 0) hp1 -= playerController.boomDamage;
            else hp2 -= playerController.boomDamage;
            StartCoroutine(onDamagedColor());
        }
        if (hp2 <= 0) 
        {
            // animator.SetInteger("State", 1);
            OnDead();            
        }
    }

    void OnDead()
    {
        onDead = true;
        UIManagerController.instance.isBossSpawn = false;
        if (gameObject.tag != "Untagged")
        {
            UIManagerController.instance.AddScore(score);
            SoundManagerController.instance.enemyDeadSound.Play();
        }
        gameObject.tag = "Untagged";
    }

    void AnimationSystem()
    {
        switch (animeNum)
        {
            case 0:
                StartCoroutine(Co_Idle());
                break;
            case 2:
                StartCoroutine(Co_LAttack());
                break;
            case 3:
                StartCoroutine(Co_RAttack());
                break;
            default:
                break;
        }
    }

    IEnumerator Co_Idle()
    {
        animeNum = -1;
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(0.6f);
    }

    IEnumerator Co_LAttack()
    {
        animeNum = -1;
        animator.SetTrigger("LAttack");
        yield return new WaitForSeconds(0.6f);
        animeNum = 0;
    }

    IEnumerator Co_RAttack()
    {
        animeNum = -1;
        animator.SetTrigger("RAttack");
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("RAttack");
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("RAttack");
        yield return new WaitForSeconds(0.6f);
        animeNum = 0;
    }

    void SpawnBoss()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition, spwanMovePos.localPosition, Time.deltaTime * speed);
        if (transform.localPosition == spwanMovePos.localPosition)
        {
            isSpawning = false;
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void FireBullet()
    {
        if (hp1 > 0 && !isSpawning)
        {
            fireDelay += Time.deltaTime;
            if (fireDelay > 1.0f && animeNum != 2)
            {
                animeNum = 2;
                fireDelay -= fireDelay;
            }
        }
        if (hp1 <= 0)
        {
            fireDelay += Time.deltaTime;
            if (fireDelay > 1.0f && animeNum != 3)
            {
                animeNum = 3;
                fireDelay -= fireDelay;
            }
        }
    }

    void LAttack()
    {
        if (player == null) return;

        Debug.Log($"LAttackPos = {LAttackPos.localPosition} ");
        Debug.Log($"LAttackPos = {LAttackPos.position} ");

        GameObject a = Instantiate(bossBullet, LAttackPos.position, Quaternion.identity);
        fireDelay -= fireDelay;
    }
    void RAttack()
    {
        if (player == null) return;

        GameObject a = Instantiate(bossBullet, RAttackPos.position, Quaternion.identity);
        Debug.Log($"{a.transform.position}");
        fireDelay -= fireDelay;
    }

    IEnumerator onDamagedColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = color;
    }
}
