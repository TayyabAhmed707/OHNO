using System;
using UnityEngine;


public class EventHandler : MonoBehaviour
{
    public delegate void DeckAction();
    public delegate void CardAction(GameObject card);
    public static event DeckAction ClickedOnDeck;
    public static event Action<string> PlayedCard;

    // Start is called before the first frame update

    public static void TriggerClickOnDeck()
    {   
        ClickedOnDeck?.Invoke();
    }
    
    public static void TriggerPlayedCard(string card)
    {
        PlayedCard?.Invoke(card);
    }

}
