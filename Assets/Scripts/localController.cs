using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine.UI;

public class localController : MonoBehaviour
{

    public Vector3 PlayerOneSpawn = new Vector3(-4.65f, 3.22f, 0);
    public Vector3 PlayerTwoSpawn = new Vector3(4.15f, 3.45f, 0);
    public Vector3 PlayerThreeSpawn = new Vector3(-6.36f, -2.07f, 0);
    public Vector3 PlayerFourSpawn = new Vector3(6.00f, -2.07f, 0);

    public Canvas scoreCanvas;
    public Text roundText;
    public Text classScoreText;
    public Text scoreScoreText;

    public Transform Player;
    private int _round = 1;

    // Use this for initialization
    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");

        scoreCanvas.enabled = false;

        if (speaker.GetComponent<AudioSource>() != null)
        {
            speaker.GetComponent<AudioSource>().Stop();
        }

        CreatePlayers(playerSelect.PlayerList.Count);
    }

    void CreatePlayers(int playerCount)
    {
        switch (playerCount)
        {
            case 2:
                GameObject go = Instantiate(Player.gameObject, PlayerOneSpawn, Quaternion.identity) as GameObject;
                go.SendMessage("PlayerNumber", 1);

                GameObject go2 = Instantiate(Player.gameObject, PlayerTwoSpawn, Quaternion.identity) as GameObject;
                go2.SendMessage("PlayerNumber", 2);
                break;
            case 3:
                GameObject go3 = Instantiate(Player.gameObject, PlayerOneSpawn, Quaternion.identity) as GameObject;
                go3.SendMessage("PlayerNumber", 1);

                GameObject go4 = Instantiate(Player.gameObject, PlayerTwoSpawn, Quaternion.identity) as GameObject;
                go4.SendMessage("PlayerNumber", 2);

                GameObject go5 = Instantiate(Player.gameObject, PlayerThreeSpawn, Quaternion.identity) as GameObject;
                go5.SendMessage("PlayerNumber", 3);
                break;
            case 4:
                GameObject go6 = Instantiate(Player.gameObject, PlayerOneSpawn, Quaternion.identity) as GameObject;
                go6.SendMessage("PlayerNumber", 1);

                GameObject go7 = Instantiate(Player.gameObject, PlayerTwoSpawn, Quaternion.identity) as GameObject;
                go7.SendMessage("PlayerNumber", 2);

                GameObject go8 = Instantiate(Player.gameObject, PlayerThreeSpawn, Quaternion.identity) as GameObject;
                go8.SendMessage("PlayerNumber", 3);

                GameObject go9 = Instantiate(Player.gameObject, PlayerFourSpawn, Quaternion.identity) as GameObject;
                go9.SendMessage("PlayerNumber", 4);
                break;
            default:
                Debug.LogError("Player count not between 1 and 4!");
                break;
        }

    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(p=> p.transform.position != Vector3.zero).ToArray();


        if (players.Length == 1)
        {
            switch (_round)
            {
                case 1:
                    Debug.Log("Round One Over " + players[0].GetComponent<playerControl>()._playerClass);
                    _round++;
                    RestartGame(players);
                    break;
                case 2:
                    Debug.Log("Round Two Over " + players[0].GetComponent<playerControl>()._playerClass);
                    _round++;
                    RestartGame(players);
                    break;
                case 3:
                    // Game over
                    Debug.Log("Game Over");
                    EndGame();
                    break;
                default:
                    Debug.LogError("Round not between 1 or 4!");
                    break;
            }
        }
    }

    void RestartGame(GameObject[] lastPlayers)
    {

        roundText.text = _round.ToString();
        classScoreText.text = lastPlayers[0].GetComponent<playerControl>()._playerClass;
        scoreScoreText.text = 1.ToString();

        foreach (var o in lastPlayers)
        {
            Destroy(o);
        }

        CreatePlayers(playerSelect.PlayerList.Count);
    }

    void EndGame()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
        Application.LoadLevel(1);
    }
}
