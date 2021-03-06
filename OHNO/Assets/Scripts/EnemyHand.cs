
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    
    public List<EnemyCard> cardComponents = new List<EnemyCard>();
    [SerializeField] private int numberOfCards = 0;
    [SerializeField] GameObject cardGameObject; // the card prefab, assigned in the editor
    [SerializeField] GameObject enemyCardGameObject;
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private Vector3 orientation = new Vector3(1,0,0);
    [SerializeField] private Vector3 depth = new Vector3(0,0,1);
    public GameObject deck;
    public TableTop tableTop;
    public float handWidth = 5f;
    public float maxgapBetweenCards = 0.5f;
    private int counter = 0;
    public Transform HandAnchor;
    public bool myTurn = true;

    //  **************  DRAW CARD METHOD  **************
    public void DrawCard()
    {
        // create a new gameobject at the position of the hand
        GameObject newCard = Instantiate(enemyCardGameObject, deck.transform.position, deck.transform.rotation);
        newCard.transform.Rotate(-90, 0, 0);
        
        cards.Add(newCard);
        cardComponents.Add(newCard.GetComponent<EnemyCard>());
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
                                          (orientation * (startPoint - i * gap - gap / 2)) +  depth*i*0.005f);
                
            cardComponents[i].rotation = HandAnchor.transform.rotation;
        }
    }
    
    public void ThrowCard(string cardToThrow)
    {
        if (cards.Count != 0)
        {
            GameObject newCard = Instantiate(cardGameObject, cards[0].transform.position, cards[0].transform.rotation);
            newCard.GetComponent<Card>().setLabel(cardToThrow);
            GameObject cardToDistroy = cards[0];
            cards.RemoveAt(0);
            Destroy(cardToDistroy);
            CalculatePositions();
            tableTop.ThrowOnTop(newCard);
        }
    }

    public void SetName(string Name)
    {
        nameDisplay.text = Name;
    }
    
}
