using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Lightbug.Kinematic2D.Implementation;
using TMPro;
using Lightbug.Kinematic2D.Core;
using System.Linq;

public enum Gamestate
{
    NEWGAME,
    LOBBY,
    MINIGAME,
    GAMEOVER
}

public class Minicats : MonoBehaviour
{
    public static Minicats instance;

    [Header("Prefabs and Data Files")]
    public GameObject MinicatsPlayerPrefab;
    public GameObject LobbyMapPrefab;
    public PlayerColors PlayerColors;
    public CatCollection CatCollection;

    [Header("Settings")]
    public int MinimumPlayers = 2; //Minimum players till the game starts.
    public int MaximumPlayers = 20; //Current 20 because of color limits.
    public int RoundsToPlay = 5;
    public float TimeTillMinigameStart = 5;

    [Header("Booleans")]
    public bool DebugLogs = true;
    public bool DebugMinigames = false;
    public bool SessionStart = false; //Application/Session Status
    public bool GameStart = false; //Overall Game Status
    public bool EnoughPlayers = false;

    bool startMinigameCountdown;

    [Header("State")]
    public Gamestate Gamestate = Gamestate.NEWGAME;
    public Game CurrentMinigame;
    public int MinigameIndex = -1;
    public int RoundsLeft = -1;

    [Header("Minigames List")]
    public List<Game> MinigamesList = new List<Game>();
    public GameCollection GameCollection;

    [Header("Lobby Cats")]
    public List<GameObject> LobbyCats = new List<GameObject>();

    //Data
    public Dictionary<int, MinicatsPlayer> Players = new Dictionary<int, MinicatsPlayer>();
    private List<Color> PlayerColorsInstance;

    private GameObject lobbyMap;

    //Counters
    private int playerCounter = -1;

    float restartTime = 10;
    float restartTimer;

    //Properties
    public int PlayerCount { get => AirConsole.instance.GetControllerDeviceIds().Count; }

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        PlayerColorsInstance = new List<Color>(PlayerColors.Colors);
    }

    private void Update()
    {
        switch (Gamestate)
        {
            case Gamestate.GAMEOVER:

                restartTimer -= Time.deltaTime;
                int t = (int)restartTimer;
                CanvasManager.instance._uiGameOver.RestartTimer.text = "Game Restarts in " + t;

                if(restartTimer <= 0)
                {
                    MinigameIndex = -1;
                    StartMinicats();
                }
                break;
            case Gamestate.LOBBY:

                if (RoundsLeft == 0)
                {
                    restartTimer = restartTime;
                    Debug.Log("[MINICATS] - Game Over.");
                    Gamestate = Gamestate.GAMEOVER;

                    if (CanvasManager.instance._uiGameOver.GotResults == false)
                    {
                        CanvasManager.instance._uiGameOver.MyCanvasGroup.alpha = 1;
                        CanvasManager.instance._uiGameOver.GetResults();
                    }
                    return;
                }

                if (PlayerCount < 2)
                {
                    EnoughPlayers = false;
                    return;
                }

                if (!EnoughPlayers)
                {
                    EnoughPlayers = true;
                    CanvasManager.instance.WaitForPlayers.alpha = 0;
                    SpawnLobbyCats();
                }

                if (!startMinigameCountdown)
                {
                    startMinigameCountdown = true;
                    StartCoroutine(LoadNextMinigame());
                }

                break;
            case Gamestate.MINIGAME:
                if (CurrentMinigame != null)
                    CurrentMinigame.Tick();
                break;
        }
    }

    IEnumerator LoadNextMinigame()
    {
        float timer = TimeTillMinigameStart;

        MinigameIndex++;
        CurrentMinigame = MinigamesList[MinigameIndex];

        //TODO: Super Refactor UI Class and a function to pass info through.
        CanvasManager.instance._uiLoadMinigame.MyCanvasGroup.alpha = 1;
        CanvasManager.instance._uiLoadMinigame.MinigameNameText.text = CurrentMinigame.ID;
        CanvasManager.instance._uiLoadMinigame.MinigameDescText.text = CurrentMinigame.Description;
        CanvasManager.instance._uiLoadMinigame.GetResults();

        while (timer > 0)
        {
            int t = (int)timer;
            CanvasManager.instance._uiLoadMinigame.Timer.text = t.ToString();
            yield return new WaitForSeconds(1);
            timer--;
        }

        CanvasManager.instance._uiLoadMinigame.MyCanvasGroup.alpha = 0;
        CurrentMinigame.Initialize();
        RemoveLobbyMap();

        Gamestate = Gamestate.MINIGAME;
        startMinigameCountdown = false;
    }

    public void RewardPlayer(int id, int reward)
    {
        foreach (var p in Players)
        {
            if (p.Value.PlayerID == id)
            {
                p.Value.Points += reward;

                if (DebugLogs)
                    Debug.Log("Gave Player " + p.Value.PlayerID + " - " + reward + " points!");
            }
        }
    }

    public MinicatsPlayer GetPlayerByID(int id)
    {
        foreach (var p in Players)
        {
            if (p.Value.PlayerID == id)
            {
                return p.Value;
            }
        }

        return null;
    }

    private void AddPlayer(int deviceID)
    {
        if (Players.ContainsKey(deviceID))
        {
            return;
        }

        if(playerCounter >= MaximumPlayers)
        {
            if (DebugLogs)
                Debug.Log("[MINICATS] - Maximum Players Reached");
            return;
        }

        //Create new MinicatsPlayer. Add as child to Manager.
        GameObject newPlayer = Instantiate(MinicatsPlayerPrefab, Vector3.zero, Quaternion.identity);
        MinicatsPlayer player = newPlayer.GetComponent<MinicatsPlayer>();
        newPlayer.transform.SetParent(transform);
        Players.Add(deviceID, player);

        //Set the player's playerID.
        playerCounter++;

        //Get a color for the player.
        int random = Random.Range(0, PlayerColorsInstance.Count - 1);
        Color c = PlayerColorsInstance[random];
        PlayerColorsInstance.Remove(c);

        //Set color to the player's device
        string colorHex = ColorToHex(c);

        string sentData = deviceID + "-" + colorHex;

        AirConsole.instance.SetCustomDeviceStateProperty("color", sentData);

        //Get player cat
        int catToUse = 0;

        if (playerCounter >= CatCollection.Cats.Count)
        {
            catToUse = playerCounter % CatCollection.Cats.Count;
        }
        else
        {
            catToUse = playerCounter;
        }

        Cats cat = CatCollection.Cats[catToUse];

        //Initialize Player
        player.InitializePlayer(playerCounter, c, cat, deviceID);

        //TODO: Queue Logic probably goes here too.
        if (Gamestate == Gamestate.LOBBY)
        {
            SpawnLobbyCat(deviceID);
        }


        if (DebugLogs)
            Debug.Log("[MINICATS] - New Player Added. Player ID: " + playerCounter);
    }

    private void RemovePlayer(int deviceID)
    {
        if (Players.ContainsKey(deviceID))
        {
            GameObject player = Players[deviceID].gameObject;

            //Destroy MinicatsControlledPlayer
            if (Players[deviceID].CurrentControlledObject != null)
                Destroy(Players[deviceID].CurrentControlledObject);

            //Destroy MinicatsPlayer
            Players.Remove(deviceID);
            Destroy(player);

            if (DebugLogs)
                Debug.Log("[MINICATS] - Player Removed. Player ID: " + playerCounter);

            playerCounter--;
        }
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    void StartMinicats()
    {
        //Restart Logic
        Gamestate = Gamestate.NEWGAME;
        CanvasManager.instance._uiGameOver.MyCanvasGroup.alpha = 0;
        CanvasManager.instance._uiGameOver.GotResults = false;

        if (LobbyCats.Count > 0)
        {
            foreach (var item in LobbyCats)
            {
                Destroy(item);
            }

            LobbyCats.Clear();
        }

        foreach (var item in Players)
        {
            item.Value.Points = 0;
        }

        EnoughPlayers = false;

        //Show New Game
        MenuManager.instance.ShowNewGame(true);

        if (DebugLogs)
            Debug.Log("[MINICATS] - Starting Game. Enjoy :>");
    }

    public void StartGame()
    {
        MenuManager.instance.ShowNewGame(false);
        Gamestate = Gamestate.LOBBY;

        if (lobbyMap)
            Destroy(lobbyMap);

        //Spawn Lobby Map
        SpawnLobbyMap();

        //Show waiting canvas
        CanvasManager.instance.WaitForPlayers.alpha = 1;

        //Init RoundsLeft
        RoundsLeft = RoundsToPlay;

        if(!DebugMinigames)
        {
            //Create Minigames List
            List<Game> gamePool = new List<Game>(GameCollection.Games);
            int length = gamePool.Count;
            MinigamesList = new List<Game>();

            for (int i = 0; i < RoundsToPlay; i++)
            {
                if (gamePool.Count <= 0)
                {
                    gamePool = new List<Game>(GameCollection.Games);
                }

                int rand = Random.Range(0, gamePool.Count);
                MinigamesList.Add(gamePool[rand]);
                gamePool.RemoveAt(rand);
            }
        }
    }

    public Vector3 GetRandomSpawnPos()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
        int rand = Random.Range(0, spawns.Length - 1);
        return spawns[rand].transform.position;
    }

    #region Lobby

    void SpawnLobbyCat(int id)
    {
        MinicatsPlayer p = Players[id];
        GameObject lobbyCat = Instantiate(p.PlayerCat.LobbyCat, Vector3.zero, Quaternion.identity);
        p.CurrentControlledObject = lobbyCat.GetComponent<ControlPlayer>();
        lobbyCat.GetComponent<ControlPlayer>().InitControlPlayer(p);

        LobbyCats.Add(lobbyCat);
    }

    public void SpawnLobbyCats()
    {
        foreach (var p in Players)
        {
            SpawnLobbyCat(p.Value.DeviceID);
        }

        if (DebugLogs)
            Debug.Log("[MINICATS] - Spawning Lobby Cats");
    }

    public void SpawnLobbyMap()
    {
        GameObject lobby = Instantiate(LobbyMapPrefab, Vector3.zero, Quaternion.identity);
        lobbyMap = lobby;

        if (DebugLogs)
            Debug.Log("[MINICATS] - Spawning Lobby Map");
    }

    void RemoveLobbyMap()
    {
        Destroy(lobbyMap);

        if (DebugLogs)
            Debug.Log("[MINICATS] - Destroying Lobby Map");
    }

    #endregion

    #region AirConsole

    private void OnReady(string code)
    {
        List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();

        foreach (int deviceID in connectedDevices)
        {
            AddPlayer(deviceID);
        }

        instance = this;
        SessionStart = true;

        StartMinicats();

        if (DebugLogs)
            Debug.Log("[MINICATS] - Session Start");
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
        if (Gamestate == Gamestate.NEWGAME)
        {
            if (Players.ContainsKey(from) && Players[from].PlayerID == 0)
            {
                if (data["action"] != null)
                {
                    var key = data["action"]["input"]["key"];
                    var pressed = data["action"]["input"]["pressed"];

                    string input = key.ToString() + pressed.ToString();

                    MenuManager.instance.GetInput(input);
                }
            }
        }

        if (Players.ContainsKey(from) && data["action"] != null)
        {
            if (data["action"]["input"] != null)
            {
                var key = data["action"]["input"]["key"];
                var pressed = data["action"]["input"]["pressed"];

                string input = key.ToString() + pressed.ToString();

                Players[from].SendInput(input);
            }
        }
    }

    #endregion

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
