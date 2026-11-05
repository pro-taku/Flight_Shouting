using UnityEngine;

public class SoundManagerController : MonoBehaviour
{
    public static SoundManagerController instance;

    public AudioSource enemyDeadSound;
    public AudioSource playerDeadSound;
    public AudioSource itemGainingSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
