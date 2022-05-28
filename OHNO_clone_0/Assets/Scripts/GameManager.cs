
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public Dictionary<int,GameObject> enemyHands;
    public GameObject ownHand;


    public void LocalplayCard(string cardplayed,char newColor)
    {
        playCard(cardplayed,newColor);
    }

    [Command] 
    public void ServerStartGame()
    {
        StartTheGame();
    }

    [Command]
    private void playCard(string cardPlayed, char newColor)
    {
        int turn = GameState.turn; 
        do
        {
            turn = (turn + 1) % GameState.numberOfPlayers;
        } while (!GameState.players.Contains(turn));
        
        
        newTurn(turn,cardPlayed,newColor);

    }

    [ClientRpc]
    private void newTurn(int turn, string topCard, char color)
    {
        GameState.turn = turn;
        GameState.topCard = topCard;
        GameState.color = color;

        
    }

    [ClientRpc]
    private void StartTheGame()
    {
        Debug.Log("Game Started!!!!");
    }
    
    [TargetRpc]
    public void TargetAssignID(NetworkConnection target, int id)
    {
        Debug.Log("GOT MY ID YAAAAAAAAAYYY");
        
    }

}   
