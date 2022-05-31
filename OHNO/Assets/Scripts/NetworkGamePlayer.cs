using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkGamePlayer : NetworkBehaviour
{

   private HandOfCards ownHand = null;
   private GameManager gameManager = null;
   
   public int myId;
   public bool isLeader;
   private int turnOrder = 1;
   private Dictionary<int, string> playerNames = new Dictionary<int, string>();

   private NetworkManagerOhno room;

   private NetworkManagerOhno Room
   {
      get
      {
         if (room != null)
         {
            return room;
         }
         return room = NetworkManager.singleton as NetworkManagerOhno;
      }
   }

   public override void OnStartServer()
   {
      NetworkManagerOhno.OnServerReadied += LeaderValidate;
   }

   private void OnEnable()
   {  
      Debug.Log("CREATED GAMEPLAYERS");
      DontDestroyOnLoad(gameObject);
   }

   

   
   
   public override void OnStopClient()
   {
      Room.GamePlayers.Remove(myId);
   }
   
   public void LeaderValidate()
   {
      
      if (isLeader)
      {
         ServerStartTheGame();
      }
   }

   [Command]
   public void ServerStartTheGame()
   {
      string[] players = new String[4];
      string firstCard = RandomCardGenerator.getRandomCard(0.0f);
      for (int i = 0; i < Room.GamePlayers.Count; i++)
      {
         setId(Room.GamePlayers[i].connectionToClient, i);
         
         players[i] = Room.GamePlayerNames[i];

      }
      
      InitialSetup(firstCard,players,Room.GamePlayers.Count);
      
      int firstTurn = Random.Range(0, Room.playerCount-1);
      
      TakeTurn(Room.GamePlayers[firstTurn].connectionToClient, firstCard,firstCard[0]);
   }

   
  
   [TargetRpc]
   void setId(NetworkConnection conn,  int id)
   {
      myId = id;
   }
   
   [TargetRpc]
   void TakeTurn(NetworkConnection conn, string topCard, char color)
   {
      Debug.Log("I got the targetRPC");
      gameManager.TakeTurn(topCard,color);
   }

   [ClientRpc]
   void InitialSetup(string card, string[] players, int numPlayers)
   {
      if (gameManager == null)
      {
         gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      }
      
      gameManager.SetupPlayerNames(players, myId, numPlayers ,this);
      gameManager.InitialSetup(card);
   }
   
   [ClientRpc]
   void EnemyTurn(int player, string card)
   {
      
   }

   [Command]
   void CmdPlayerReady()
   {
      
   }
   
   
   // DRAWING CARDS
   
   [Command(requiresAuthority = false)]
   public void iDrewCard(int player)
   {
      playerDrewCard(player);
   }
   
   [ClientRpc]
   private void playerDrewCard(int player)
   {
      gameManager.playerDrewCard(player);
   }
   public override void OnStartClient()
   {
      
      myId = Room.playerCount;
      Room.GamePlayers[Room.playerCount] = this;
      Room.playerCount++;
   }
   
   // Playing CARDS
   
   [Command(requiresAuthority = false)]
   public void iPlayedCard(int player, string card, char color, int turn)
   {
      int nextTurn = ((player + turn * turnOrder) % Room.GamePlayers.Count);
    
      if (turn == -1)
      {
         turnOrder *= -1;
      }

      if (card == "w0")
      {
         drawNCards(Room.GamePlayers[nextTurn].connectionToClient,4);
         playerDrewNCards(nextTurn,4);
      }
      else if (card.Substring(1) == "11")
      {
         drawNCards(Room.GamePlayers[nextTurn].connectionToClient,2);
         playerDrewNCards(nextTurn,2);
      }
      playerPlayedCard(player, card, color);
      TakeTurn(Room.GamePlayers[nextTurn].connectionToClient, card, card[0]);
   }
   


   [ClientRpc]
   private void playerPlayedCard(int player,string card, char color)
   {
      gameManager.playerPlayedCard(player, card, color);
   }
   
   // Winning logic
   
   [Command(requiresAuthority = false)]
   public void iWon(string Name)
   {
      playerWon(Name);
      NetworkManager.singleton.ServerChangeScene("Scene_Lobby");
   }


   [ClientRpc]
   private void playerWon(string player)
   {
      gameManager.playerWon(player);
   }
   
   
   [TargetRpc]
   private void drawNCards(NetworkConnection conn,int howMany)
   {
      gameManager.DrawNCards(howMany);
   }
   
   [ClientRpc]
   private void playerDrewNCards(int player,int howmany)
   {
      gameManager.playerDrewCards(player, howmany);
   }
}


