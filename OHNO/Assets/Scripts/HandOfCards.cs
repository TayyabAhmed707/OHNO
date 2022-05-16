
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class HandOfCards : MonoBehaviour
{

    //[SerializeField] private GameObject[] cards = new GameObject[150];  //array of the card gameObjects
    public Dictionary<string, GameObject> cards = new Dictionary<string, GameObject>();
    //private Card[] cardComponents = new Card[150];  //array of the card script attached to the cards, (to avoid using getComponent again and again) 
    public Dictionary<string, Card> cardComponents  = new Dictionary<string, Card>();
    [SerializeField] private int numberOfCards = 0;
    [SerializeField] GameObject cardGameObject;     // the card prefab, assigned in the editor

    public GameObject deck;
    
    public float handBalanceSpeed = 1;
    public float rotationSpeed = 1;
    public float handWidth = 5f;
    public float maxgapBetweenCards = 0.2f;
    private int counter = 0;
    public Transform deckAnchor;     
    void DrawCard(float pWild = 0.2f)
    {
        // generate a random card
        string newCardLabel = RandomCardGenerator.getRandomCard(pWild);
        
        // create a new gameobject at the position of the hand
        GameObject newCard = Instantiate(cardGameObject, deck.transform.position, deck.transform.rotation);
        newCard.transform.Rotate(-90,0,0);
        cards[newCardLabel] = newCard;
        cardComponents[newCardLabel] = newCard.GetComponent<Card>();
        
        cardComponents[newCardLabel].setLabel(newCardLabel);
        newCard.transform.parent = transform;
        numberOfCards++;
        
        CalculatePositions();
        
    }

    private void CalculatePositions()
    {
        float gap = numberOfCards * maxgapBetweenCards > handWidth ? handWidth / numberOfCards : maxgapBetweenCards;

        float startPoint = gap * numberOfCards/2.0f;
        int i = 0;
        foreach(KeyValuePair<string,Card> c in cardComponents)
        {
            c.Value.setPosition(deckAnchor.transform.position + new Vector3(startPoint - i * gap - gap/2 ,0  ,i*0.02f));
            i++;
        }
        
    }

    
    private void Update()
    {
        
        counter++;
        if (counter > 200)
        {
            counter = 0;
            DrawCard();
        }
    }
}
