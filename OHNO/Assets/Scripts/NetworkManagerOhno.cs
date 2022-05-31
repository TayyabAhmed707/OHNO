using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerOhno : NetworkManager
{
    private int playerInLobby;
    public int playerCount = 0;
    [SerializeField] private int minPalyers = 2;
    [SerializeField] private string menuScene;
    [Header("Room")] [SerializeField] private NetworkRoomPlayer roomPlayerPrefab;

    [Header("Game")] [SerializeField] private NetworkGamePlayer gamePlayerPrefab;
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action OnServerReadied;
    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();

    public Dictionary<int, NetworkGamePlayer> GamePlayers { get; } = new Dictionary<int, NetworkGamePlayer>();
    public Dictionary<int, string> GamePlayerNames { get; } = new Dictionary<int, string>();
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();


    public int GetPlayerCount()
    {
        return GamePlayers.Count;
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();

    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();

    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers > maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);


        }

    }


    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();
            RoomPlayers.Remove(player);
            NotifyPlayerOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }


    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        base.OnStopServer();
    }

    public void NotifyPlayerOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPalyers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }

            ServerChangeScene("Scene_Game1");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        playerInLobby = RoomPlayers.Count;
        //Menu to game
        if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Game"))
        {

            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                GamePlayerNames[i] = RoomPlayers[i].DisplayName;
                gameplayerInstance.isLeader = RoomPlayers[i].IsLeader;
                
                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);

            }
        }

        base.ServerChangeScene(newSceneName);
    }


    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        if (playerInLobby > 1)
        {
            playerInLobby--;
        }
        else
        {
            OnServerReadied?.Invoke();
        }
    }
}

   

