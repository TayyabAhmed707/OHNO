using System;
using System.Linq;
using System.Net;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayer : NetworkBehaviour
{
   [Header("UI")] 
   [SerializeField] private GameObject lobbyUI;

   [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
   [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
   [SerializeField] private Button startGameButton;
   [SerializeField] private TMP_Text hostIp;
   
   [SyncVar(hook = nameof(HandleDisplayNameChanged))]
   public string DisplayName = "loading...";

   [SyncVar(hook = nameof(HandleReadyStatusChanged))]
   public bool IsReady = false;


   private bool isLeader;

   private void OnEnable()
   {
      DontDestroyOnLoad(gameObject);
   }

   public bool IsLeader
   {
      get { return isLeader;}
      set
      {
         isLeader = value;
         startGameButton.gameObject.SetActive(value);
      }
      
   }

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

   public override void OnStartAuthority()
   {
      CmdSetDisplayName(PlayerNameEnter.DisplayName);
      lobbyUI.SetActive(true);
      hostIp.text = "Host IP address: " + GetLocalIPv4();
   }

   public override void OnStartClient()
   {
      Room.RoomPlayers.Add(this);
      UpdateDisplay();
   }

   public override void OnStopClient()
   {
      Room.RoomPlayers.Remove(this);
      UpdateDisplay();
   }

   public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
   public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

   private void UpdateDisplay()
   {
      if (!hasAuthority)
      {
         foreach (var player in Room.RoomPlayers)
         {
            if (player.hasAuthority)
            {
               player.UpdateDisplay();
               break;
            }
         }

         return;
      }
      
      for (int i = 0; i < playerNameTexts.Length; i++)
      {
         playerNameTexts[i].text = "Waiting For player...";
         playerReadyTexts[i].text = string.Empty;
      }

      for (int i = 0; i < Room.RoomPlayers.Count; i++)
      {
         playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
         playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady?
            "<color=green> Ready</color>":
            "<color=red> Not Ready</color>";
      }
   }
   
   public string GetLocalIPv4()
   {
      return Dns.GetHostEntry(Dns.GetHostName())
         .AddressList.First(
            f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
         .ToString();
   }    

   public void HandleReadyToStart(bool readyToStart)
   {
      if(!isLeader){return;}

      startGameButton.interactable = readyToStart;
   }

   [Command]
   private void CmdSetDisplayName(string displayName)
   {
      DisplayName = displayName;
   }

   [Command]
   public void CmdReadyUp()
   {
      IsReady = !IsReady;
      Room.NotifyPlayerOfReadyState();
   }

   [Command]
   public void CmdStartGame()
   {
      if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
      {
         return;
      }
      
      Room.StartGame();
   }
   
}