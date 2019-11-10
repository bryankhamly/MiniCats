using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Lightbug.Kinematic2D.Implementation;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform SpawnPoint;

    public Dictionary<int, CharacterHybridBrain> Players = new Dictionary<int, CharacterHybridBrain>();
    public int PlayerCount { get => AirConsole.instance.GetControllerDeviceIds().Count; }

    bool _sessionStarted;
    bool _gameStart;

    bool _playingMinigame;

    int _playerCounter = -1;
    int _countdownTimer;

    private void Update()
    {
        if (_sessionStarted)
        {
            if (!_gameStart && PlayerCount >= 2 && !_playingMinigame)
            {
                StartGame();
            }

            if (_gameStart && PlayerCount < 2)
            {
                StopGame();
            }
        }
    }

    void StartGame()
    {
        _gameStart = true;
        _countdownTimer = 5;
        StartCoroutine(Countdown());
        Debug.Log("Game Start");
    }

    void StopGame()
    {
        _gameStart = false;
        Debug.Log("Game Stop");
    }

    void StartMinigame()
    {
        _playingMinigame = true;
        Debug.Log("Start Minigame");
    }

    void StopMinigame()
    {
        _playingMinigame = false;
        Debug.Log("Stop Minigame");
    }

    IEnumerator Countdown()
    {
        while (_countdownTimer >= 0)
        {
            Debug.Log("Countdown: " + _countdownTimer);
            yield return new WaitForSeconds(1);
            _countdownTimer--;        
        }

        StartMinigame();
        Debug.Log("Countdown Finish");
    }

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void AddPlayer(int deviceID)
    {
        if (Players.ContainsKey(deviceID))
        {
            return;
        }

        GameObject newPlayer = Instantiate(PlayerPrefab, SpawnPoint.position, transform.rotation) as GameObject;
        Players.Add(deviceID, newPlayer.GetComponent<CharacterHybridBrain>());

        _playerCounter++;
        newPlayer.GetComponent<Player>().PlayerID = _playerCounter;

        Debug.Log("DEBUG: New Player Added: " + deviceID);
    }

    private void RemovePlayer(int deviceID)
    {
        if (Players.ContainsKey(deviceID))
        {
            GameObject player = Players[deviceID].gameObject;
            Players.Remove(deviceID);
            Destroy(player);

            _playerCounter--;

            Debug.Log("DEBUG: Player Removed: " + deviceID);
        }
    }

    private void OnReady(string code)
    {
        List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
        foreach (int deviceID in connectedDevices)
        {
            AddPlayer(deviceID);
        }

        _sessionStarted = true;
    }

    private void OnConnect(int device)
    {
        AddPlayer(device);
    }

    private void OnDisconnect(int device)
    {
        RemovePlayer(device);
    }

    private void OnMessage(int from, JToken data)
    {
        if (Players.ContainsKey(from) && data["action"] != null)
        {
            Players[from].ButtonInput(data["action"].ToString());
        }
    }

    private void OnDestroy()
    {
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
            AirConsole.instance.onReady -= OnReady;
            AirConsole.instance.onConnect -= OnConnect;
            AirConsole.instance.onDisconnect -= OnDisconnect;
        }
    }
}
