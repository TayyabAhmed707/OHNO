
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class TableTop : MonoBehaviour
{
    
    
    private static Queue<GameObject> cardQueue = new Queue<GameObject>();
    public Transform TableTopAnchor;
    public GameObject deck;
    public GameObject cardGameObject;
    public string topCard = "";
    
    public void ThrowOnTop(GameObject card)
    {   
        cardQueue.Enqueue(card);
        topCard = card.GetComponent<Card>().getLabel();

        GameManager.cardThrown(topCard);

        Card cardComponent = card.GetComponent<Card>();
        
        cardComponent.position = TableTopAnchor.position + new Vector3(0,0.5f,0);
        cardComponent.movingUsingCode =false;
        cardComponent.rotation = TableTopAnchor.rotation;
        cardComponent.rotation *=  Quaternion.Euler(Vector3.forward*UnityEngine.Random.Range(-15f, 15f));
        if (cardQueue.Count > 10)
        {
            Destroy(cardQueue.Dequeue());
        }
    }

    public void drawFirstCard()
    {
        // generate a random card
        string newCardLabel = RandomCardGenerator.getRandomCard(0.0f);
        
        // create a new gameobject at the position of the hand
        GameObject newCard = Instantiate(cardGameObject, deck.transform.position, deck.transform.rotation);
        
        newCard.GetComponent<Card>().setLabel(newCardLabel);
        ThrowOnTop(newCard);
        
    }


    private void OnEnable()
    {
        drawFirstCard();
        
    }
}
