using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 총알
    public GameObject[] bullets;
    float fireDelay;

    // 움직임
    public float speed;
    public Vector2 limitMax;
    public Vector2 limitMin;
    Vector2 move;

    // 애니메이션
    Animator animator;
    bool onDaed;
    float animaRunTime;

    // 아이템
    public int damage;
    public int boom;
    public GameObject boomMissile;
    public int boomPosY;
    public int boomDamage;

    void Start()
    {
        fireDelay = 0;

        onDaed = false;
        animaRunTime = 0;
        animator = GetComponent<Animator>();

        damage = 1;
        boom = 0;

        boomPosY = -30;
        boomDamage = 30;
    }

    // Update is called once per frame
    void Update()
    {
        TranslatePos();
        FireBullet();
        OnDeadCheck();
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        FireBoom();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            animator.SetInteger("State", 1);
            onDaed = true;
        }
    }

    void TranslatePos()
    {
        float x = move.x * speed * Time.deltaTime;
        float y = move.y * speed * Time.deltaTime;

        Vector3 pos = transform.localPosition + new Vector3(x, y, 0);

        if (pos.x >= limitMax.x) pos.x = limitMax.x;
        if (pos.y >= limitMax.y) pos.y = limitMax.y;
        if (pos.x <= limitMin.x) pos.x = limitMin.x;
        if (pos.y <= limitMin.y) pos.y = limitMin.y;

        transform.localPosition = pos;
    }

    void FireBullet()
    {
        fireDelay += Time.deltaTime;

        if (fireDelay > 0.3f)
        {
            Instantiate(bullets[damage-1], transform.localPosition, Quaternion.identity);
            fireDelay -= 0.3f;
        }
    }

    void FireBoom()
    {
        if (boom >= 1)
        {
            GameObject go = Instantiate(boomMissile, transform.localPosition, Quaternion.identity);
            go.transform.localPosition = new Vector3(transform.localPosition.x, boomPosY, transform.localPosition.z);
            boom--;
            UIManagerController.instance.CheckBoom(boom);
        }
    }

    void OnDeadCheck()
    {
        if (onDaed)
        {
            if (SoundManagerController.instance.playerDeadSound.isPlaying == false)
            {
                SoundManagerController.instance.playerDeadSound.Play();
            }
            animaRunTime += Time.deltaTime;
        }
        if (animaRunTime > 0.6f)
        {
            Destroy(gameObject);

            GameManageerController.instance.RemovePlayerLife();
            GameManageerController.instance.CreatePlayer();
            UIManagerController.instance.CheckLife(GameManageerController.instance.life);
            GameManageerController.instance.GameOverCheck();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(limitMin, new Vector2(limitMax.x, limitMin.y));
        Gizmos.DrawLine(limitMin, new Vector2(limitMin.x, limitMax.y));
        Gizmos.DrawLine(limitMax, new Vector2(limitMax.x, limitMin.y));
        Gizmos.DrawLine(limitMax, new Vector2(limitMin.x, limitMax.y));
    }
}
