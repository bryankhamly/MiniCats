using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Lightbug.Kinematic2D.Implementation;
using TMPro;

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

    public GameObject[] Cats;
    public Transform SpawnPoint;

    public List<PlayerData> PlayerData = new List<PlayerData>();

    public Dictionary<int, CharacterHybridBrain> Players = new Dictionary<int, CharacterHybridBrain>();
    public int PlayerCount { get => AirConsole.instance.GetControllerDeviceIds().Count; }

    public List<int> Queue = new List<int>();

    public List<Player> PlayerList = new List<Player>();

    public int CountdownTime = 10;
    public int RoundsToPlay = 5;

    public Minigame[] Minigames;
    public Minigame CurrentMinigame;
    int _minigameIndex = -1;

    //UI
    public CanvasGroup StartCanvas;
    public CanvasGroup MinigameCountdown;
    public TextMeshProUGUI TimerText;

    public TextMeshProUGUI MinigameTitle;
    public TextMeshProUGUI MinigameDesc;
    public TextMeshProUGUI MinigameTimer;

    public CanvasGroup FoodCanvas;

    public CanvasGroup QueueCanvas;
    public TextMeshProUGUI QueueText;

    public CanvasGroup WinnerCanvas;
    public TextMeshProUGUI WinnerText;
    public TextMeshProUGUI NextGameText;

    bool _sessionStarted;
    bool _gameStart;

    bool _playingMinigame;

    int _playerCounter = -1;
    int _countdownTimer;

    private void Start()
    {
        
    }

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

    public void ShowNametags(bool yes)
    {
        foreach (var item in PlayerList)
        {
            item.ShowNametag(yes);
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
        MinigameTitle.text = CurrentMinigame.ID;
        MinigameDesc.text = CurrentMinigame.Description;
    }

    void StartMinigame()
    {   
        _playingMinigame = true;
        CurrentMinigame.Initialize();
        Debug.Log("MINIGAMES: Starting: " + CurrentMinigame.ID);
    }

    public void StopMinigame()
    {
        CurrentMinigame = null;
        _playingMinigame = false;
        SpawnQueuedPlayers();
        Debug.Log("MINIGAMES: Stop");
    }

    public void ShowWinner(bool show, int winnerID = -1)
    {
        if(show)
        {
            WinnerCanvas.alpha = 1;
            WinnerText.text = "Winner: Player " + winnerID;
            NextGameText.text = "Next minigame: " + Minigames[_minigameIndex + 1].ID + " starts in {0} seconds...";    
        }
        else
        {
            WinnerCanvas.alpha = 0;
        }
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
            QueueCanvas.alpha = 0;
        }
    }

    IEnumerator Countdown()
    {
        while (_countdownTimer >= 0)
        {
            Debug.Log("GAMESTATE: Countdown: " + _countdownTimer + ", till the game starts.");
            TimerText.text = "Game starts in " + _countdownTimer + " seconds...";
            yield return new WaitForSeconds(1);
            _countdownTimer--;          
        }

        StartCanvas.alpha = 0;
        SetMinigame(_minigameIndex + 1);

        StartCoroutine(CountdownToMinigame());

        Debug.Log("COUNTDOWN: Countdown Finish");
    }

    IEnumerator CountdownToMinigame()
    {
        MinigameCountdown.alpha = 1;
        _countdownTimer = CurrentMinigame.StartTime;

        while (_countdownTimer >= 0)
        {
            Debug.Log("GAMESTATE: Countdown: " + _countdownTimer + ", till a minigame starts.");
            MinigameTimer.text = CurrentMinigame.ID + " starts in " + _countdownTimer + " seconds...";
            yield return new WaitForSeconds(1);
            _countdownTimer--;
        }

        MinigameCountdown.alpha = 0;
        StartMinigame();
        Debug.Log("COUNTDOWN: Countdown to Minigame Finish");
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

        _playerCounter++;

        int catToUse = _playerCounter;

        //If 5th player: 4 >= 4 cats (length) = 0
        //If 6th player: 5 >= 4 cats = 1
        //catToUse needs to be 0, so catToUse - length

        if(catToUse >= Cats.Length)
        {
            catToUse = catToUse - Cats.Length;
        }

        GameObject newPlayer = Instantiate(Cats[catToUse], SpawnPoint.position, transform.rotation) as GameObject;
        Players.Add(deviceID, newPlayer.GetComponent<CharacterHybridBrain>());

        newPlayer.GetComponent<Player>().SetPlayerID(_playerCounter);

        PlayerList.Add(newPlayer.GetComponent<Player>());

        PlayerData.Add(new PlayerData(_playerCounter));

        Debug.Log("DEBUG: New Player Added: " + deviceID);
    }

    private void RemovePlayer(int deviceID)
    {
        if (Players.ContainsKey(deviceID))
        {
            GameObject player = Players[deviceID].gameObject;
            Players.Remove(deviceID);
            PlayerList.Remove(player.GetComponent<Player>());
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
        StartCanvas.alpha = 1;
    }

    private void OnConnect(int device)
    {
        if(_gameStart && _playingMinigame)
        {
            //Add to queue.
            Queue.Add(device);
            Debug.Log("QUEUE: Added a player to the queue: " + device);
            QueueText.text = "Players in Queue: " + Queue.Count;
            QueueCanvas.alpha = 1;
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
