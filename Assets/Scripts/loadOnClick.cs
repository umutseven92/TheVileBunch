using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public void ResetPlayers()
    {
        playerSelect.playerCount = 0;
        playerSelect.playerList = new List<playerSelect.Player>();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
