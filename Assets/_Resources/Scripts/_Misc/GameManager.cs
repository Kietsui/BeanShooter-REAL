using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkManager
{
    public static GameManager Instance;  // Singleton instance of the GameManager
    public List<GameObject> players = new List<GameObject>();  // List of all connected players
    public int maxPlayers = 2;  // Max number of players

    public GameStateManager gameStateManager; // Reference to the GameStateManager

    // Called when the server starts
    public override void OnStartServer()
    {
        base.OnStartServer();
        Instance = this;  // Set the instance of GameManager
        Debug.Log("Server started.");

        // Find or create the GameStateManager
        if (gameStateManager == null)
        {
            GameObject gameStateObj = new GameObject("GameStateManager");
            gameStateManager = gameStateObj.AddComponent<GameStateManager>();
            NetworkServer.Spawn(gameStateObj); // Ensure it's spawned on the server
        }
    }

    // Called when a client connects to the server
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Player connected: " + conn);
    }

    // Called when a player is added to the server
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // Add the new player to the player list
        GameObject newPlayer = conn.identity.gameObject;
        players.Add(newPlayer);
        Debug.Log("Player added. Current player count: " + players.Count);

        // Start the game when the max number of players is reached
        if (players.Count >= maxPlayers && gameStateManager.currentState == GameStateManager.GameState.WaitingForPlayers)
        {
            StartCoroutine(StartGame());
        }
    }

    // Called when a client disconnects from the server
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        GameObject disconnectedPlayer = conn.identity.gameObject;
        players.Remove(disconnectedPlayer);
        Debug.Log("Player disconnected. Current player count: " + players.Count);

        base.OnServerDisconnect(conn);
    }

    [Server]
    public IEnumerator StartGame()
    {
        Debug.Log("Starting game...");
        gameStateManager.SetGameState(GameStateManager.GameState.GameInProgress);

        // Initialize game logic, countdown, etc.
        yield return new WaitForSeconds(3);

        Debug.Log("Game started!");
    }

    [Server]
    public void EndGame()
    {
        Debug.Log("Game over.");
        gameStateManager.SetGameState(GameStateManager.GameState.GameOver);

        // Handle game over logic
    }

    [Server]
    public void CheckGameOverCondition()
    {
        // Example logic: If one player remains, end the game
        if (players.Count == 1)
        {
            EndGame();
        }
    }
}
