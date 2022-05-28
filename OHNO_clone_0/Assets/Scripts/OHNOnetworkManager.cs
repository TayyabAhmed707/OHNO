using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OHNOnetworkManager : NetworkManager
{
   public GameManager gm;
   
   public int numberOfClients = 0;
   private Dictionary<int, NetworkConnectionToClient> clients = new Dictionary<int, NetworkConnectionToClient>();

   
   public override void OnStartHost()
   {
      Debug.Log("I am The host");
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public override void OnServerConnect(NetworkConnectionToClient conn)
   {
      base.OnServerConnect(conn);
      
      Debug.Log("ClientConnected");
      Debug.Log(conn.connectionId);
      
      if (numberOfClients > 4)
      {
         conn.Disconnect();
      }
      
      clients[numberOfClients] = conn;
      numberOfClients++;
      
   }

   public NetworkConnectionToClient GetNetworkConnection(int playerId)
   {
      return clients[playerId];
   }
   
   
   
   
}
