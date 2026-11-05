using System.Diagnostics;
using UnityEngine;

public class BoomMisslieController : MonoBehaviour
{
    public float speed;
    float time;

    void Start()
    {
        speed = 45f;
        time = 0;
    }

    void Update()
    {
        MoveBoom();
        DestroyBoom();
    }

    void MoveBoom()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void DestroyBoom()
    {
        time += Time.deltaTime;
        if (time > 3f)
        {
            Destroy(gameObject);
        }
    }
}
