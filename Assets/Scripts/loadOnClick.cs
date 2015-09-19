using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class loadOnClick : MonoBehaviour
{

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }


    public void ResetPlayers()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndLocalGame()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
        Application.LoadLevel(1);
    }
}
