using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;


public static class GameState
{   
    public static int turn = 0;
    public static int numberOfPlayers = 0;
    public static List<int> players = new List<int>();
    public static string topCard = null;
    public static char color;
    public static bool turnReversed = false;


    public static bool IsValidMove(string card)
    {
        return card[0] == 'w' || card[0] == color || card.Substring(1) == topCard.Substring(1);
    }
    
    

}
