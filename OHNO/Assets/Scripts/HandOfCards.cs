
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class HandOfCards : MonoBehaviour
{

    [SerializeField] private GameObject[] cards = new GameObject[150];  //array of the card gameObjects
    private Card[] cardComponents = new Card[150];  //array of the card script attached to the cards, (to avoid using getComponent again and again) 
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
        cards[numberOfCards] = newCard;
        cardComponents[numberOfCards] = newCard.GetComponent<Card>();
        
        cardComponents[numberOfCards].setLabel(newCardLabel);
        newCard.transform.parent = transform;
        numberOfCards++;
        
        CalculatePositions();
        
    }

    private void CalculatePositions()
    {
        float gap = numberOfCards * maxgapBetweenCards > handWidth ? handWidth / numberOfCards : maxgapBetweenCards;

        float startPoint = (gap / 2 * (numberOfCards - 1));
        for (int i = 0; i < numberOfCards; i++)
        {
            cardComponents[i].setPosition(deckAnchor.transform.position + new Vector3(startPoint - i * gap ,0  ,i*0.02f));
            
        }
        
    }

    private void UpdateCardPosition()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            cards[i].transform.position = Vector3.MoveTowards(cards[i].transform.position,
                cardComponents[i].position, Time.deltaTime * handBalanceSpeed);

            cards[i].transform.rotation = Quaternion.Lerp(cards[i].transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);

        }
        
        
    }
    private void Update()
    {
        
        counter++;
        UpdateCardPosition();
        if (counter > 200)
        {
            
            counter = 0;
            DrawCard();
        }
    }
}
