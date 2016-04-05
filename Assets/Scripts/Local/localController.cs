using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
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

    public Button btnEndGameExit;
    public Button btnExit;
    public Text winnerText;
    public Text CountdownText;
    public Transform Player;
    public AudioSource musicPlayer;
    public AudioSource dingPlayer;

    public Transform AmmoPickup;
    public Transform HealthPickup;
    public Transform[] pickupSpawns; // Spawn objects for pickups

    public int ScoreToReach = 3;
    private int _round = 1;

    private bool gameOver = false;
    private double _counter = -1.000d;
    private double gameOverCounter = 0.000d;
    private double gameOverMs = 3.000d;

    private bool slowMo = false;
    private double slowMoCounter = 0.000d;
    public double slowMoMs = 1.500d;

    private double scoreCardMs = 3.000d;

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

    private double _roundOverCounter;
    public double _roundOverMs;
    private bool start = false;

    // Use this for initialization
    void Start()
    {
        if (Time.timeScale < 1.0f)
        {
            Time.timeScale = 1f;
        }

        scoreCanvas.enabled = false;
        endGameCanvas.enabled = false;
        pauseCanvas.enabled = false;

        CheckPlayerPrefs();

        CreatePlayers(playerSelect.PlayerList.Count);
        SetScoreCard();

        foreach (var s in pickupSpawns)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;
        }

        ammoMs = CalculateNewAmmoRange();
        healthMs = CalculateNewHealthRange();
    }

    private void CheckPlayerPrefs()
    {
        musicPlayer.mute = PlayerPrefs.GetInt(global.Music) != 1;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && scoreCanvas.enabled == false)
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

        if (players.Length == 1)
        {
            winner = new KeyValuePair<string, int>(players[0].GetComponent<localPlayer>()._playerClass, 3);
            slowMo = true;
            gameOver = true;
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
    }

    void PauseAllPlayers()
    {
        if (gameOver)
        {
            return;
        }

        var allPlayers = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

        foreach (var allPlayer in allPlayers)
        {
            allPlayer.SendMessage("Pause");
        }

    }

    void UnPauseAllPlayers()
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

        foreach (var allPlayer in allPlayers)
        {
            allPlayer.SendMessage("UnPause");
        }
    }

    void SetEndGameCard(string __winner)
    {
        winnerText.text = __winner;
        PauseAllPlayers();
        endGameCanvas.enabled = true;
        btnEndGameExit.Select();
    }

    void SetScoreCard()
    {
        musicPlayer.Pause();
        dingPlayer.loop = true;
        dingPlayer.Play();
        start = true;
    }

    private void ScoreCardTimer()
    {
        if (start)
        {
            PauseAllPlayers();
            _counter += 1 * Time.deltaTime;

            if (_counter >= 0 && _counter < 1.000)
            {
                scoreCanvas.enabled = true;
                CountdownText.text = "3";
            }
            if (_counter >= 1.000 && _counter < 2.000)
            {
                CountdownText.text = "2";
            }
            if (_counter >= 2.000 && _counter < scoreCardMs)
            {
                CountdownText.text = "1";
            }

            if (_counter >= 2.500 && _counter < scoreCardMs)
            {
                dingPlayer.Stop();
            }

            if (_counter >= scoreCardMs)
            {

                scoreCanvas.enabled = false;
                start = false;
                musicPlayer.UnPause();
                UnPauseAllPlayers();
                _counter = 0.000d;
            }
        }
    }

    void PickupTimers()
    {
        AmmoTimer();
        //HealthTimer();
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

                localSceneHelper.Winner = winner.Key;
                SceneManager.LoadScene("GraveyardLocal");

                //SetEndGameCard(winner.Key);
            }
        }
    }

}
