using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int Turn = 1;
    public static int myPlayerId = 1;
    public static List<int> players = new List<int>();
    public static string TopCard = null;
    public static char Color;

    public static bool isValidMove(string card)
    {
        return card[0] == 'w' || card[0] == Color || card.Substring(1) == TopCard.Substring(1);
    }

    public static void cardThrown(string card)
    {
        TopCard = card;
        if (card[0] != 'w')
        {
            Color = card[0];
        }
    }

    public static void changeColor() 
    {
        
    }

    public static void rotateTurn()
    {
        
    }
}
