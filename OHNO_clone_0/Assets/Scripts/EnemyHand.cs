
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    
    public List<EnemyCard> cardComponents = new List<EnemyCard>();
    [SerializeField] private int numberOfCards = 0;
    [SerializeField] GameObject cardGameObject; // the card prefab, assigned in the editor
    [SerializeField] GameObject enemyCardGameObject;
    public GameObject deck;
    public TableTop tableTop;
    public float handWidth = 5f;
    public float maxgapBetweenCards = 0.2f;
    private int counter = 0;
   public Transform HandAnchor;
    public bool myTurn = true;

    
    //  **************  DRAW CARD METHOD  **************
    void DrawCard(float pWild = 0.2f)
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
    
    // **************  CALCULATES POSITIONS WHEN DRAWING **************
    private void CalculatePositions()
    {
        float gap = numberOfCards * maxgapBetweenCards > handWidth ? handWidth / numberOfCards : maxgapBetweenCards;
        float startPoint = gap * numberOfCards / 2.0f;

        for (int i = 0; i < numberOfCards; i++)
        {
            cardComponents[i].setPosition(HandAnchor.transform.position +
                                          new Vector3(startPoint - i * gap - gap / 2, i * -0.02f, i * 0.02f));
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
    
}
