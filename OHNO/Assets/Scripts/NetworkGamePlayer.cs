using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkGamePlayer : NetworkBehaviour
{

   private HandOfCards ownHand = null;
   private int myId;

   [SyncVar]
   private string displayName = "loading...";

   [SyncVar] private bool GameOver = false;
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
      Debug.Log("Subscribing to  First CArd");
      NetworkManagerOhno.FirstCardDrawn += FirstCard;
   }

   private void OnEnable()
   {  
      Debug.Log("CREATED GAMEPLAYERS");
      DontDestroyOnLoad(gameObject);
   }

   public override void OnStartClient()
   {
      
      myId = Room.playerCount;
      Room.GamePlayers[Room.playerCount] = this;
      Room.playerCount++;
   }
   
   public override void OnStopClient()
   {
      Room.GamePlayers.Remove(myId);
   }
   
   [Server]
   public void FirstCard(string card)
   {
      
      Debug.Log("Sucess in invoking");
      DrawFirstCard(card);
      int firstTurn = Random.Range(0, Room.playerCount-1);
      TakeTurn(Room.GamePlayers[firstTurn].connectionToClient,card);
   }

   
   
   [Server]
   public void SetDisplayName(string displayName)
   {
      this.displayName = displayName;
   }

   [TargetRpc]
   void TakeTurn(NetworkConnection conn, string topCard)
   {
      if (ownHand == null)
      {
         ownHand = GameObject.Find("HandOfCards").GetComponent<HandOfCards>();
         
      }
      ownHand.TakeTurn(topCard);
   }

   [ClientRpc]
   void DrawFirstCard(string card)
   {  
      Debug.Log("FIRST CARRRRRRRRDS");
      GameObject.Find("TableTop").GetComponent<TableTop>().drawFirstCard(card);
   }
   
   [ClientRpc]
   void EnemyTurn(int player, string card)
   {
      
   }

   [Command]
   void CmdPlayerReady()
   {
      
   }

}


