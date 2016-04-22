using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class graveyardOnlineScript : graveyardScript
{

    public Text OneOnline;
    public Text[] TwoOnline;
    public Text[] ThreeOnline;

    public Button BtnPlayAgain;

    private bool playAgain;

    protected override void Start()
    {
        base.Start();
        WinnerText.text = WinnerText.text.Replace("{class}", onlineSceneHelper.Winner);
        var remaining = playerSelect.PlayerList;

        remaining.Remove(remaining.Find(p => p.Class.Equals(onlineSceneHelper.Winner)));

        switch (playerSelect.PlayerList.Count - 1)
        {
            case 3:
                Three[0].text = remaining[0].Class;
                ThreeGrave[0].GetComponent<SpriteRenderer>().enabled = true;
                ThreeCanvas[0].GetComponent<Canvas>().enabled = true;

                Three[1].text = remaining[1].Class;
                ThreeGrave[1].GetComponent<SpriteRenderer>().enabled = true;
                ThreeCanvas[1].GetComponent<Canvas>().enabled = true;

                Three[2].text = remaining[2].Class;
                ThreeGrave[2].GetComponent<SpriteRenderer>().enabled = true;
                ThreeCanvas[2].GetComponent<Canvas>().enabled = true;

                ThreeOnline[0].text = remaining[0].OnlinePlayerName;
                ThreeOnline[1].text = remaining[1].OnlinePlayerName;
                ThreeOnline[2].text = remaining[2].OnlinePlayerName;
                break;
            case 2:
                Two[0].text = remaining[0].Class;
                TwoGrave[0].GetComponent<SpriteRenderer>().enabled = true;
                TwoCanvas[0].GetComponent<Canvas>().enabled = true;

                Two[1].text = remaining[1].Class;
                TwoGrave[1].GetComponent<SpriteRenderer>().enabled = true;
                TwoCanvas[1].GetComponent<Canvas>().enabled = true;

                TwoOnline[0].text = remaining[0].OnlinePlayerName;
                TwoOnline[1].text = remaining[1].OnlinePlayerName;
                break;
            case 1:
                One.text = remaining[0].Class;
                OneGrave.GetComponent<SpriteRenderer>().enabled = true;
                OneCanvas.GetComponent<Canvas>().enabled = true;

                OneOnline.text = remaining[0].OnlinePlayerName;
                break;
        }

        BtnPlayAgain.onClick.AddListener(PlayAgainButtonPressed);
    }

    private void PlayAgainButtonPressed()
    {
        BtnPlayAgain.enabled = false;

        playAgain = true;
    }

    protected override void Update()
    {
        base.Update();
        if (done)
        {
            if (playAgain)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    playerSelect.PlayerList = new List<playerSelect.Player>();
                    PhotonNetwork.automaticallySyncScene = false;
                    Time.timeScale = 1;
                    PhotonNetwork.LoadLevel("OnlinePlayerSelect");
                }
            }
            else
            {
                GetBackToMenu();
            }
        }
    }

    private static void GetBackToMenu()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
        PhotonNetwork.automaticallySyncScene = false;
        Time.timeScale = 1;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }

}
