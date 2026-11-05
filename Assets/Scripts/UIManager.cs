using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerController : MonoBehaviour
{
    // Signle Ton
    public static UIManagerController instance;

    // UI_Boom
    public GameObject[] uiBooms;

    // UI_Score
    public Text uiScore;
    public Text uiHighScore;
    public int score;
    public int highScore;

    // UI_Life
    public GameObject[] uiLifes;

    // BlackOut_Curtain
    public Image blackOutCurtain;
    float blackOutValue;
    float blackOutspeed;

    // UI_GameOver
    public Image gameOverImage;

    // Boss
    public Image hpFrame;
    public Image hp1;
    public Image hp2;
    public float maxHp1;
    public float maxHp2;
    public BossController bossController;
    public bool isBossSpawn;

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
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        blackOutValue = 1f;
        blackOutspeed = 0.5f;
        isBossSpawn = false;
    }

    void Update()
    {
        if (blackOutValue > 0)
        {
            HideBlackOutCurtain();
        }
        if (isBossSpawn)
        {
            CheckBossHp();
        }
        else
        {
            hpFrame.gameObject.SetActive(false);
            hp1.gameObject.SetActive(false);
            hp2.gameObject.SetActive(false);
        }
    }

    public void CheckBoom(int count)
    {
        for (int i=0 ; i < uiBooms.Length ; i++)
        {
            uiBooms[i].SetActive(i+1 <= count);
        }
    }

    public void CheckLife(int count)
    {
        for (int i=0 ; i < uiLifes.Length ; i++)
        {
            uiLifes[i].SetActive(i+1 <= count);
        }
    }

    public void AddScore(int i)
    {
        score += i;
        uiScore.text = score.ToString();
    }

    public void HideBlackOutCurtain()
    {
        blackOutValue -= Time.deltaTime * blackOutspeed;
        blackOutCurtain.color = new Color(0f, 0f, 0f, blackOutValue);
    }

    public void GameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
        uiHighScore.text = highScore.ToString();
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title_Scene");

        Destroy(UIManagerController.instance.gameObject);
        Destroy(GameManageerController.instance.gameObject);
        Destroy(SoundManagerController.instance.gameObject);
    }

    public void CheckBossHp()
    {
        hpFrame.gameObject.SetActive(true);
        hp2.gameObject.SetActive(true);
        hp1.gameObject.SetActive(true);

        hp2.fillAmount = bossController.hp2 / maxHp2;
        hp1.fillAmount = bossController.hp1 / maxHp1;
    }
}
