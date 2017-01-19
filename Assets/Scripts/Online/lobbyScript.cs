using System;
using log4net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lobbyScript : MonoBehaviour
{
    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void CreateLobby()
    {
        var lobbyName = Guid.NewGuid().ToString();

        onlineHelper.LobbyName = lobbyName;
        onlineHelper.Joining = false;

        Log.InfoFormat("Joining {0}..",lobbyName);
        SceneManager.LoadScene("OnlineLoading");
    }
}
