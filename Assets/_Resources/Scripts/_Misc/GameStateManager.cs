using UnityEngine;
using Mirror;

public class GameStateManager : NetworkBehaviour
{
    public enum GameState { WaitingForPlayers, GameInProgress, GameOver }

    [SyncVar]
    public GameState currentState = GameState.WaitingForPlayers;

    [Server]
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game state changed to: " + newState);
    }
}
