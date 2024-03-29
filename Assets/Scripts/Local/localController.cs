﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class localController : MonoBehaviour
{
    [HideInInspector]
    public int score = 0;

    public GameObject Fade;
    private const double MAX_ALPHA = 255f;
    private double alphaPerSec;
    public float SlowMoScale = 0.2f;

    public Vector3[] PlayerSpawns;

    public Canvas scoreCanvas;
    public Canvas pauseCanvas;

    public Button btnExit;
    public Text CountdownText;

    public Transform Freeman;
    public Transform Cowboy;
    public Transform Dancer;
    public Transform Prospector;

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
        pauseCanvas.enabled = false;

        CheckPlayerPrefs();

        // Testing the levels
        if (Debug.isDebugBuild && playerSelect.PlayerList.Count == 0)
        {
            CreatePlayers(1);
        }

        CreatePlayers(playerSelect.PlayerList.Count);
        SetScoreCard();

        foreach (var s in pickupSpawns)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;
        }

        ammoMs = CalculateNewAmmoRange();

        alphaPerSec = (MAX_ALPHA / (slowMoMs * (1 / SlowMoScale)) / 50);

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


        var players = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();
        CheckTimers();

        if (Debug.isDebugBuild)
        {
            players = CheckDevModeEndGame(players);
        }

        if (players.Length == 1)
        {
            // Game over
            winner = new KeyValuePair<string, int>(players[0].GetComponent<localPlayer>()._playerClass, 3);
            slowMo = true;
            gameOver = true;
        }
    }

    /// <summary>
    /// End game early
    /// </summary>
    /// <param name="players"></param>
    /// <returns></returns>
    private GameObject[] CheckDevModeEndGame(GameObject[] players)
    {
        if (Input.GetButtonDown("DevMode"))
        {
            return new[] { players[0] };
        }
        return players;
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


    int CalculateNewAmmoRange()
    {
        return _rand.Next(ammoMsLower, ammoMsUpper);
    }

    void CreatePlayers(int playerCount)
    {
        for (int i = 1; i <= playerCount; i++)
        {
            string pClass = string.Empty;

            if (Debug.isDebugBuild && playerSelect.PlayerList.Count == 0)
            {
                pClass = "The Cowboy";
            }
            else
            {
                pClass = playerSelect.PlayerList[i - 1].Class;
            }

            switch (pClass)
            {
                case "The Cowboy":
                    InstatiatePlayer(Cowboy.gameObject, i);
                    break;
                case "The Dancer":
                    InstatiatePlayer(Dancer.gameObject, i);
                    break;
                case "The Freeman":
                    InstatiatePlayer(Freeman.gameObject, i);
                    break;
                case "The Prospector":
                    InstatiatePlayer(Prospector.gameObject, i);
                    break;
            }

        }
    }

    private void InstatiatePlayer(GameObject player, int iteration)
    {
        GameObject go = Instantiate(player, PlayerSpawns[iteration - 1], Quaternion.identity) as GameObject;

        /*
        if (iteration % 2 == 0)
        {
            go.GetComponent<SpriteRenderer>().flipX = true;
        }
        */
        go.SendMessage("PlayerNumber", iteration);
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


    private void SlowMotionTimer()
    {
        if (slowMo)
        {
            Time.timeScale = SlowMoScale;
            musicPlayer.pitch = 0.5f;
            slowMoCounter += 1 * Time.deltaTime;

            var tmp = Fade.GetComponent<SpriteRenderer>().color;
            tmp.a += float.Parse(alphaPerSec.ToString()) * Time.deltaTime;

            Fade.GetComponent<SpriteRenderer>().color = tmp;

            if (slowMoCounter >= slowMoMs)
            {
                Time.timeScale = 1f;
                musicPlayer.pitch = 1f;
                slowMo = false;
                slowMoCounter = 0.000d;

                localSceneHelper.Winner = winner.Key;
                SceneManager.LoadScene("GraveyardLocal");
            }
        }
    }

}
