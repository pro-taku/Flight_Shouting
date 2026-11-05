using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public Transform[] enemySpawns;
    public GameObject[] enemyObjects;

    float time;
    float respawnTime;
    int enemyCount;
    int[] randomIndexs;
    int wave;   // TODO : 나중에 사용

    bool createdBoss;
    public GameObject boss;

    void Start()
    {
        time = 0;
        respawnTime = 4.0f;
        enemyCount = 5;
        randomIndexs = new int[enemyCount];
        wave = 0;
        createdBoss = false;
    }

    void Update()
    {
        Timer();
        if (wave >= 5 && !createdBoss)
        {
            CreateBoss();
        }
    }

    void Timer()
    {
        time += Time.deltaTime;
        if (time > respawnTime)
        {
            RandomPos();
            CreateEnemy();
            wave++;
            time -= time;   // 미세한 오차를 줄이기 위한 초기화 방법
        }
    }

    void RandomPos()
    {
        for (int i=0 ; i<enemyCount ; i++)
        {
            randomIndexs[i] = Random.Range(0,9);
        }
    }

    void CreateEnemy()
    {
        if (GameManageerController.instance.life < 0)
            return;

        for (int i=0 ; i<enemyCount ; i++)
        {
            int cnt = Random.Range(0, enemyObjects.Length);
            GameObject obj = GameObject.Instantiate(enemyObjects[cnt]);
            obj.transform.localPosition = enemySpawns[randomIndexs[i]].localPosition;

            float x = obj.transform.localPosition.x;
            float result = Random.Range(x - 2f, x + 2f);
            obj.transform.localPosition = new Vector3(result, obj.transform.localPosition.y, transform.localPosition.z);
        }
    }

    void CreateBoss()
    {
        createdBoss = true;
        GameObject temp = Instantiate(boss);
        int randomIndex = Random.Range(0, 9);
        temp.transform.localPosition = enemySpawns[randomIndex].localPosition;
        BossController bossController = temp.GetComponent<BossController>();

        UIManagerController.instance.isBossSpawn = true;
        UIManagerController.instance.maxHp1 = bossController.hp1;
        UIManagerController.instance.maxHp2 = bossController.hp2;
        UIManagerController.instance.bossController = bossController;
    }
}
