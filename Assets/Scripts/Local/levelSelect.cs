using System.Linq;
using UnityEngine;
using log4net;
using UnityEngine.SceneManagement;

public class levelSelect : MonoBehaviour
{
    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void LoadLevel(string levelLoading)
    {
        if (string.IsNullOrEmpty(levelLoading))
        {
            global.LogDebug(Log, "Level doesn't exist yet.");
            return;
        }

        foreach (var p in playerSelect.PlayerList.Where(p => !p.Set))
        {
            playerSelect.PlayerList.Remove(p);
        }

        SceneManager.LoadScene(levelLoading);
    }

    public void LoadOnlineLevel(string level)
    {
        if (string.IsNullOrEmpty(level))
        {
            global.LogDebug(Log, "Level doesn't exist yet.");
            return;
        }

     //   PhotonNetwork.LoadLevel(level);
    }
   
}
