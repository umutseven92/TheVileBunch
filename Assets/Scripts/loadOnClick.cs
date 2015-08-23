using UnityEngine;
using System.Collections;

public class loadOnClick : MonoBehaviour {

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
