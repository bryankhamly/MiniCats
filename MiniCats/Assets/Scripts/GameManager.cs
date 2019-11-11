using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Lightbug.Kinematic2D.Implementation;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public PlayerData(int id)
    {
        ID = id;
        Points = 0;
    }

    public int ID;
    public int Points;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject PlayerPrefab;
    public Transform SpawnPoint;

    public List<PlayerData> PlayerData = new List<PlayerData>();

    public Dictionary<int, CharacterHybridBrain> Players = new Dictionary<int, CharacterHybridBrain>();
    public int PlayerCount { get => AirConsole.instance.GetControllerDeviceIds().Count; }

    public List<int> Queue = new List<int>();

    public int CountdownTime = 10;

    public Minigame[] Minigames;
    public Minigame CurrentMinigame;

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

            if(_gameStart && _playingMinigame)
            {
                if(CurrentMinigame != null)
                {
                    CurrentMinigame.Tick();
                }
            }
        }
    }

    void StartGame()
    {
        _gameStart = true;
        _countdownTimer = CountdownTime;
        StartCoroutine(Countdown());
        Debug.Log("GAMESTATE: Game Start");
    }

    void StopGame()
    {
        _gameStart = false;
        Debug.Log("GAMESTATE: Game Stop");
    }

    void SetMinigame(int index)
    {
        CurrentMinigame = Minigames[index];
        CurrentMinigame.Initialize();
    }

    void StartMinigame()
    {
        SetMinigame(0);
        _playingMinigame = true;
        Debug.Log("MINIGAMES: Starting: " + CurrentMinigame.ID);
    }

    public void StopMinigame()
    {
        CurrentMinigame = null;
        _playingMinigame = false;
        SpawnQueuedPlayers();
        Debug.Log("MINIGAMES: Stop");
    }

    void SpawnQueuedPlayers()
    {
        if (Queue.Count > 0)
        {
            Debug.Log("Players in Queue: " + Queue.Count);

            for (int i = 0; i < Queue.Count; i++)
            {
                AddPlayer(Queue[i]);
                Debug.Log("Spawning player from queue: " + Queue[i]);
            }

            Queue.Clear();
        }
    }

    IEnumerator Countdown()
    {
        while (_countdownTimer >= 0)
        {
            Debug.Log("GAMESTATE: Countdown: " + _countdownTimer + ", till a minigame starts.");
            yield return new WaitForSeconds(1);
            _countdownTimer--;        
        }

        StartMinigame();
        Debug.Log("COUNTDOWN: Countdown Finish");
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
        newPlayer.GetComponent<Player>().SetPlayerID(_playerCounter);

        PlayerData.Add(new PlayerData(_playerCounter));

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

        instance = this;

        _sessionStarted = true;
    }

    private void OnConnect(int device)
    {
        if(_gameStart && _playingMinigame)
        {
            //Add to queue.
            Queue.Add(device);
            Debug.Log("QUEUE: Added a player to the queue: " + device);
        }
        else
        {
            AddPlayer(device);
        }     
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
