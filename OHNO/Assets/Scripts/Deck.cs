using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private HandOfCards ownHand;
    public bool isClickable = false;
    void OnMouseOver()
    {   
        
        
        if (isClickable && Input.GetMouseButtonDown(0))
        {
            if (GameManager.MyTurn)
            { 
                EventHandler.TriggerClickOnDeck();
            }
            
        }
                
    }
    
}
