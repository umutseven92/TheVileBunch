using UnityEngine;
using System.Collections;

public class loadOnClick : MonoBehaviour
{

    public static int playerCount = 0;

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void SetPlayerCount(int _playerCount)
    {
        playerCount = _playerCount;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
