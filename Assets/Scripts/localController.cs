using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class localController : MonoBehaviour
{
    [HideInInspector]
    public int score = 0;

    public Vector3 PlayerOneSpawn = new Vector3(-6.98f, 3.06f, 0);
    public Vector3 PlayerTwoSpawn = new Vector3(7.02f, 3.06f, 0);
    public Vector3 PlayerThreeSpawn = new Vector3(-7.28f, -1.1f, 0);
    public Vector3 PlayerFourSpawn = new Vector3(7.25f, -1.1f, 0);
    public Canvas scoreCanvas;
    public Canvas endGameCanvas;
    public Canvas pauseCanvas;

    public Button btnExit;
    public Text staticRoundText;
    public Text roundText;
    public Text classScoreText;
    public Text scoreScoreText;
    public Text winnerText;
    public Transform Player;
    public AudioSource musicPlayer;

    public Transform AmmoPickup;
    public Transform HealthPickup;
    public Transform[] pickupSpawns; // Spawn objects for pickups

    private int _round = 1;

    private bool gameOver = false;
    private double _counter = 0.000d;
    private double gameOverCounter = 0.000d;
    private double gameOverMs = 3.000d;
    private bool overtime = false;

    private bool slowMo = false;
    private double slowMoCounter = 0.000d;
    public double slowMoMs = 1.500d;

    public double scoreCardMs = 1.500d;

    private Dictionary<string, int> ClassScores = new Dictionary<string, int>();
    private KeyValuePair<string, int> winner;

    private bool paused = false;
    private readonly Random _rand = new Random();

    private double _healthCounter = 0.000d;
    private double _ammoCounter = 0.000d;

    public int ammoMsUpper = 30;
    public int ammoMsLower = 10;
    private int ammoMs;

    public int healthMsUpper = 40;
    public int healthMsLower = 20;
    private int healthMs;

    private bool ammoOnScreen = false;
    private bool healthOnScreen = false;

    private bool roundOver;
    private double _roundOverCounter;
    public double _roundOverMs;

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
        pauseCanvas.enabled = false;

        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        foreach (playerSelect.Player t in playerSelect.PlayerList)
        {
            ClassScores.Add(t.Class, 0);
        }

        CreatePlayers(playerSelect.PlayerList.Count);
        SetScoreCard(1.ToString(), "   Get Ready!", string.Empty);

        foreach (var s in pickupSpawns)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;
        }

        ammoMs = CalculateNewAmmoRange();
        healthMs = CalculateNewHealthRange();

    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (paused)
            {
                UnPauseAllPlayers();
          
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

                Time.timeScale = 1;
                pauseCanvas.enabled = false;
                musicPlayer.UnPause();
            }
            else
            {
                PauseAllPlayers();
                btnExit.Select();
                Time.timeScale = 0;
                pauseCanvas.enabled = true;
                musicPlayer.Pause();
            }
            paused = !paused;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();
        CheckTimers();

        // Overtime
        if (players.Length == 1 && !gameOver && overtime)
        {
            ClassScores[players[0].GetComponent<playerControl>()._playerClass]++;

            Dictionary<string, int> winners = ClassScores.Where(classScore => classScore.Value == ClassScores.Values.Max()).ToDictionary(classScore => classScore.Key, classScore => classScore.Value);
            winner = winners.First();
            slowMo = true;
            gameOver = true;
        }

        if (players.Length == 1 && !gameOver && !overtime)
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
                    Dictionary<string, int> winners = ClassScores.Where(classScore => classScore.Value == ClassScores.Values.Max()).ToDictionary(classScore => classScore.Key, classScore => classScore.Value);

                    if (winners.Count == 1)
                    {
                        // One winner
                        winner = winners.First();
                        slowMo = true;
                        gameOver = true;
                    }
                    else if (winners.Count == 3)
                    {
                        // Three winners, sudden death
                        overtime = true;
                        playerSelect.Player loser = new playerSelect.Player();
                        if (playerSelect.PlayerList.Count > winners.Count)
                        {
                            foreach (var p in playerSelect.PlayerList)
                            {
                                if (!winners.Keys.Contains(p.Class))
                                {
                                    loser = p;
                                    break;
                                }
                            }

                            Debug.Log(loser.Class + " disqualified.");
                            playerSelect.PlayerList.Remove(loser);
                        }

                        ClassScores = new Dictionary<string, int>();
                        foreach (playerSelect.Player t in playerSelect.PlayerList)
                        {
                            ClassScores.Add(t.Class, 0);
                        }

                        RestartGame(players);
                    }
                    else
                    {
                        Debug.LogError(winners.Count + " players are tied!");
                    }

                    break;
                default:
                    Debug.LogError("Round not between 1 or 4!");
                    break;
            }
        }

    }

    void SpawnRandomAmmo(Transform pickup)
    {
        var pos = _rand.Next(0, pickupSpawns.Length);
        var spawn = pickupSpawns[pos];

        if (GameObject.FindGameObjectsWithTag("ammoPickup").Length <= 0)
        {
            var hPickup = GameObject.FindGameObjectWithTag("healthPickup");

            if (hPickup == null)
            {
                Instantiate(pickup, new Vector3(spawn.position.x, spawn.position.y, spawn.position.z), Quaternion.identity);
            }
            else
            {
                if (new Vector2(hPickup.transform.position.x, hPickup.transform.position.y) != new Vector2(spawn.position.x, spawn.position.y))
                {
                    Instantiate(pickup, new Vector3(spawn.position.x, spawn.position.y, spawn.position.z), Quaternion.identity);
                }
            }
        }
    }

    void SpawnRandomHealth(Transform pickup)
    {
        var pos = _rand.Next(0, pickupSpawns.Length);
        var spawn = pickupSpawns[pos];

        if (GameObject.FindGameObjectsWithTag("healthPickup").Length <= 0)
        {
            var aPickup = GameObject.FindGameObjectWithTag("ammoPickup");

            if (aPickup == null)
            {
                Instantiate(pickup, new Vector3(spawn.position.x, spawn.position.y, spawn.position.z), Quaternion.identity);

            }
            else
            {
                if (new Vector2(aPickup.transform.position.x, aPickup.transform.position.y) != new Vector2(spawn.position.x, spawn.position.y))
                {
                    Instantiate(pickup, new Vector3(spawn.position.x, spawn.position.y, spawn.position.z), Quaternion.identity);
                }
            }
        }
    }

    int CalculateNewAmmoRange()
    {
        return _rand.Next(ammoMsLower, ammoMsUpper);
    }

    int CalculateNewHealthRange()
    {
        return _rand.Next(healthMsLower, healthMsUpper);
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


    void CheckTimers()
    {
        ScoreCardTimer();
        SlowMotionTimer();
        PickupTimers();
        PlayerSpawnTimer();
    }

    void RestartGame(GameObject[] lastPlayers)
    {
        if (overtime)
        {
            SetScoreCard(string.Empty, "Sudden Death", string.Empty);
        }
        else
        {
            SetScoreCard(_round.ToString(), lastPlayers[0].GetComponent<playerControl>()._playerClass, ClassScores[lastPlayers[0].GetComponent<playerControl>()._playerClass].ToString());
        }

        foreach (var o in lastPlayers)
        {
            Destroy(o);
        }

        foreach (var s in GameObject.FindGameObjectsWithTag("Slash"))
        {
            Destroy(s);
        }

        _healthCounter = 0;
        _ammoCounter = 0;

        // Destroy ammo & health pickups
        GameObject.FindGameObjectsWithTag("ammoPickup").ToList().ForEach(Destroy);
        GameObject.FindGameObjectsWithTag("healthPickup").ToList().ForEach(Destroy);

        roundOver = true;
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
        if (overtime)
        {
            staticRoundText.enabled = false;
        }
        else
        {
            staticRoundText.enabled = true;
        }
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

    private void PlayerSpawnTimer()
    {
        if (roundOver)
        {
            _roundOverCounter += Time.deltaTime;
            if (_roundOverCounter >= _roundOverMs)
            {
                roundOver = false;
                CreatePlayers(playerSelect.PlayerList.Count);
                _roundOverCounter = 0d;
            }
        }
    }

    void PickupTimers()
    {
        AmmoTimer();
        HealthTimer();
    }

    private void AmmoTimer()
    {
        _ammoCounter += 1 * Time.deltaTime;

        if (_ammoCounter >= ammoMs)
        {
            SpawnRandomAmmo(AmmoPickup);
            _ammoCounter = 0;
            ammoMs = CalculateNewAmmoRange();
        }

    }

    private void HealthTimer()
    {
        _healthCounter += 1 * Time.deltaTime;

        if (_healthCounter >= healthMs)
        {
            SpawnRandomHealth(HealthPickup);
            _healthCounter = 0;
            healthMs = CalculateNewHealthRange();
        }
    }

    private void SlowMotionTimer()
    {
        if (slowMo)
        {
            Time.timeScale = 0.2f;
            musicPlayer.pitch = 0.5f;
            slowMoCounter += 1 * Time.deltaTime;

            if (slowMoCounter >= slowMoMs)
            {
                Time.timeScale = 1f;
                musicPlayer.pitch = 1f;
                slowMo = false;
                slowMoCounter = 0.000d;

                // One winner
                SetEndGameCard(winner.Key);

            }
        }
    }

}
