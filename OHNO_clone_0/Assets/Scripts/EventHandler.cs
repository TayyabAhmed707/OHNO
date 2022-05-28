using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EventHandler : MonoBehaviour
{
    public delegate void DeckAction();
    public delegate void CardAction(GameObject card);
    public static event DeckAction ClickedOnDeck;
    public static event CardAction ClickedOnCard;
    
    
    // Start is called before the first frame update

    public static void TriggerClickOnDeck()
    {
        ClickedOnDeck?.Invoke();
    }
    
}
