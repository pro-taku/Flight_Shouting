using UnityEngine;

public class GameManageerController : MonoBehaviour
{
    // Singleton
    public static GameManageerController instance;

    // Player
    public GameObject playerPrefab;
    PlayerController playerController;
    Vector3 playerPos;

    // Life
    public int life;

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

    void Start()
    {
        life = 2;
        UIManagerController.instance.CheckLife(life);
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        if (life >= 0)
        {
            GameObject player = Instantiate(playerPrefab);
            
            float x = Random.Range(-9f, 9f);
            float y = -18f;
            player.transform.localPosition = new Vector3(x, y, 0);
            
            playerController = player.GetComponent<PlayerController>();
            
            UIManagerController.instance.CheckBoom(playerController.boom);
        }
    }

    public void RemovePlayerLife()
    {
        life--;
    }

    public void GameOverCheck()
    {
        if (life < 0)
        {
            UIManagerController.instance.GameOver();
        }
    }
}
