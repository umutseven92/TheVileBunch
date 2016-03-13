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
            Log.Debug("Level doesn't exist yet.");
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
            Log.Debug("Level doesn't exist yet.");
            return;
        }

        PhotonNetwork.LoadLevel(level);
    }
}
