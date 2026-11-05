using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    float height;
    float speed;
    BoxCollider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        height = collider2D.size.y;
        speed = 3.0f;
    }

    void Update()
    {
        Move();
        if (transform.localPosition.y <= -height)
        {
            Reposition();
        }
    }

    void Move()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void Reposition()
    {
        Vector3 offset = new(0, height*2, 0);
        transform.localPosition = transform.localPosition + offset;
    }
}
