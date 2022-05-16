using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TableTop : MonoBehaviour
{
    
    
    private static Queue<GameObject> cardQueue = new Queue<GameObject>();
    public GameObject deck;
    public GameObject cardGameObject;
    public string topCard = "";
    
    public void ThrowOnTop(GameObject card)
    {   
        cardQueue.Enqueue(card);
        topCard = card.GetComponent<Card>().getLabel();
        
        if (cardQueue.Count > 69)
        {
            Destroy(cardQueue.Dequeue());
        }
    }

    public void drawFirstCard()
    {
        // generate a random card
        string newCardLabel = RandomCardGenerator.getRandomCard(0);
        
        // create a new gameobject at the position of the hand
        GameObject newCard = Instantiate(cardGameObject, deck.transform.position, deck.transform.rotation);
        
        ThrowOnTop(newCard);
        newCard.GetComponent<Card>().position = transform.position;
    }
    
}
