using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine.UI;

public class localController : MonoBehaviour
{

    public Vector3 PlayerOneSpawn = new Vector3(-6.98f, 3.06f, 0);
    public Vector3 PlayerTwoSpawn = new Vector3(7.02f, 3.06f, 0);
    public Vector3 PlayerThreeSpawn = new Vector3(-7.28f, -1.1f, 0);
    public Vector3 PlayerFourSpawn = new Vector3(7.25f, -1.1f, 0);

    public Canvas scoreCanvas;
    public Canvas endGameCanvas;
    public Text roundText;
    public Text classScoreText;
    public Text scoreScoreText;
    public Text winnerText;

    public Transform Player;
    private int _round = 1;
    private double _counter = 0.000d;
    private double gameOverCounter = 0.000d;
    private double scoreCardMs = 1.500d;
    private double gameOverMs = 3.000d;
    private bool slowMo = false;
    private double slowMoCounter = 0.000d;
    private double slowMoMs = 1.500d;
    private bool gameOver = false;

    [HideInInspector]
    public int score = 0;

    private Dictionary<string, int> ClassScores = new Dictionary<string, int>();

    // Use this for initialization
    void Start()
    {

        if (Time.timeScale < 1.0f)
        {
            Time.timeScale = 1f;
        }
        GameObject speaker = GameObject.Find("Speaker");

        scoreCanvas.enabled = false;
        endGameCanvas.enabled = false;

        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        foreach (playerSelect.Player t in playerSelect.PlayerList)
        {
            ClassScores.Add(t.Class,0);
        }

        CreatePlayers(playerSelect.PlayerList.Count);
        SetScoreCard(1.ToString(),string.Empty, string.Empty);

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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

        CheckTimers(players);

        if (players.Length == 1)
        {
            ClassScores[players[0].GetComponent<playerControl>()._playerClass]++;

            switch (_round)
            {
                case 1:
                    _round++;
                    RestartGame(players);
                    break;
                case 2:
                    _round++;
                    RestartGame(players);
                    break;
                case 3:
                    // Game over
                    slowMo = true;
                    gameOver = true;
                    break;
                default:
                    Debug.LogError("Round not between 1 or 4!");
                    break;
            }
        }
    }

    void CheckTimers(GameObject[] _players)
    {

        ScoreCardTimer();
        SlowMotionTimer();
        GameOverTimer();
    }

    void RestartGame(GameObject[] lastPlayers)
    {
        SetScoreCard(_round.ToString(), lastPlayers[0].GetComponent<playerControl>()._playerClass, ClassScores[lastPlayers[0].GetComponent<playerControl>()._playerClass].ToString());

        foreach (var o in lastPlayers)
        {
            Destroy(o);
        }

        foreach (var s in GameObject.FindGameObjectsWithTag("Slash"))
        {
            Destroy(s);
        }

        CreatePlayers(playerSelect.PlayerList.Count);
    }

    void PauseAllPlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

        foreach (var allPlayer in allPlayers)
        {
            allPlayer.SendMessage("Pause");
        }
        
    }

    void UnPauseAllPlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

        foreach (var allPlayer in allPlayers)
        {
            allPlayer.SendMessage("UnPause");
        }
        
    }


    void SetEndGameCard(string winner)
    {
        winnerText.text = winner;
        PauseAllPlayers();
        endGameCanvas.enabled = true;
    }

    void SetScoreCard(string round, string winner, string score)
    {
        roundText.text = round;
        classScoreText.text = winner;
        scoreScoreText.text = score;

        scoreCanvas.enabled = true;
    }

    private void ScoreCardTimer()
    {
        if (scoreCanvas.enabled)
        {
            PauseAllPlayers();
            _counter += 1 * Time.deltaTime;

            if (_counter >= scoreCardMs)
            {
                scoreCanvas.enabled = false;
                UnPauseAllPlayers();
                _counter = 0.000d;

            }
        }
    }

    private void SlowMotionTimer()
    {
        if (slowMo)
        {
            Time.timeScale = 0.2f;
            slowMoCounter+= 1 * Time.deltaTime;

            if (slowMoCounter >= slowMoMs)
            {
                Time.timeScale = 1f;
                slowMo = false;
                slowMoCounter = 0.000d;

                if (ClassScores.Keys.Count(k => ClassScores[k] == ClassScores.Values.Max()) > 1)
                {
                    Debug.Log("Multiple Winners!");  
                }

                string winner = ClassScores.Keys.First(k => ClassScores[k] == ClassScores.Values.Max());
                SetEndGameCard(winner);

            }
        }
    }


    private void GameOverTimer()
    {
        if (gameOver)
        {
            gameOverCounter+= 1 * Time.deltaTime;


            if (gameOverCounter >= gameOverMs)
            {
                gameOver= false;
                gameOverCounter = 0.000d;
            }
        }
    }


}
