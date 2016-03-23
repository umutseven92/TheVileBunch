using System.Globalization;
using log4net;
using UnityEngine.SceneManagement;

public class lobbyScript : Photon.PunBehaviour
{
    private string LobbyName { get; set; }

    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void CreateLobby()
    {
        if (string.IsNullOrEmpty(LobbyName))
        {
            Log.Info("Lobby name is empty.");
            return;
        }

        onlineHelper.LobbyName = LobbyName;
        onlineHelper.Joining = false;

        Log.InfoFormat("Joining {0}..",LobbyName);
        SceneManager.LoadScene("OnlineLoading");
    }

    public void SetLobbyName(string lobbyName)
    {
        LobbyName = lobbyName.ToLower(CultureInfo.InvariantCulture);
    }
}
