using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Game_Scene");
    }
}
