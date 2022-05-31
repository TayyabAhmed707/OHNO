

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HandOfCards : MonoBehaviour
{

    //[SerializeField] private GameObject[] cards = new GameObject[150];  //array of the card gameObjects
    public List<GameObject> cards = new List<GameObject>();

    //private Card[] cardComponents = new Card[150];  //array of the card script attached to the cards, (to avoid using getComponent again and again) 
    public List<Card> cardComponents = new List<Card>();
    [SerializeField] public int numberOfCards = 0;
    [SerializeField] GameObject cardGameObject; // the card prefab, assigned in the editor

    public GameObject deck;
    public TableTop tableTop;
    public float handWidth = 5f;
    public float maxgapBetweenCards = 0.2f;
    [FormerlySerializedAs("deckAnchor")] public Transform HandAnchor;


    //  **************  DRAW CARD METHOD  **************
    public void DrawCard(float pWild = 0.0f)
    {
        // generate a random card
        string newCardLabel = RandomCardGenerator.getRandomCard(pWild);

        // create a new gameobject at the position of the hand
        GameObject newCard = Instantiate(cardGameObject, deck.transform.position, deck.transform.rotation);
        newCard.transform.Rotate(-90, 0, 0);
        
        cards.Add(newCard);
        cardComponents.Add(newCard.GetComponent<Card>());

        cardComponents[numberOfCards].setLabel(newCardLabel);
        cardComponents[numberOfCards].Hand = this;
        newCard.transform.parent = transform;
        numberOfCards++;

        CalculatePositions();

    }

    public void DrawCards(int howMany, float pWild = 0.2f)
    {
        for (int i = 0; i < howMany; i++)
        {
            DrawCard();
        }
    }
    
    // **************  CALCULATES POSITIONS WHEN DRAWING **************
    private void CalculatePositions()
    {
        float gap = numberOfCards * maxgapBetweenCards > handWidth ? handWidth / numberOfCards : maxgapBetweenCards;
        float startPoint = gap * numberOfCards / 2.0f;

        for (int i = 0; i < numberOfCards; i++)
        {
            cardComponents[i].setPosition(HandAnchor.transform.position +
                                          new Vector3(startPoint - i * gap - gap / 2, -i*0.007f, i * 0.02f));
            cardComponents[i].rotation = HandAnchor.transform.rotation;
        }
    }

    public void TakeTurn(string topCard, char color)
    {
        tableTop.topCard = topCard;
        if (color == '.')
        {
            tableTop.color = topCard[0];
        }
        else
        {
            tableTop.color = color;
        }
    }

    void ClickedOnDeck()
    {
        if (GameManager.MyTurn)
        {
            DrawCard();
        }
        
    }

    public void ThrowCard(GameObject cardToThrow)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (cards[i] == cardToThrow)
            {
                cardComponents.Remove(cardComponents[i]);
                GameObject card = cards[i];
                cards.Remove(cards[i]);
                numberOfCards = cards.Count;
                tableTop.ThrowOnTop(card);
                CalculatePositions();
                break;
            }
        }
    }
    
    // ************** SUBSCRIBING AND UNSUBSCRIBING FROM EVENTS ********************
    private void OnEnable()
    {
        EventHandler.ClickedOnDeck += ClickedOnDeck;
    }

    private void OnDisable()
    {
        EventHandler.ClickedOnDeck -= ClickedOnDeck;
    }
}
