using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    void OnMouseOver()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            EventHandler.TriggerClickOnDeck();
        }
                
    }
    
}
