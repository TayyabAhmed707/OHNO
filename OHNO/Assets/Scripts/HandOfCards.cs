using System;
using UnityEditor.UI;
using UnityEngine;

public class HandOfCards : MonoBehaviour
{

    [SerializeField] private GameObject[] cards = new GameObject[150];
    [SerializeField] private int number_of_cards = 0;
    [SerializeField] GameObject cardGameObject;

    public float handWidth = 5f;
    public float maxgapBetweenCards = 0.2f;
    
    private int counter = 0;
    void DrawCard(float pWild = 0.2f)
    {
        string newCardLabel = RandomCardGenerator.getRandomCard(pWild);
        GameObject newCard = Instantiate(cardGameObject, transform.position, Quaternion.identity);
        newCard.GetComponent<Card>().setLabel(newCardLabel);

        cards[number_of_cards] = newCard;
        number_of_cards++;
        
        calculatePositions();
        
    }

    private void calculatePositions()
    {
        float gap = number_of_cards * maxgapBetweenCards > handWidth ? handWidth / number_of_cards : maxgapBetweenCards;

        float startPoint = -(gap / 2 * (number_of_cards - 1));
        for (int i = 0; i < number_of_cards; i++)
        {
            cards[i].GetComponent<Card>().setPosition(new Vector3(startPoint + i * gap ,0 ,0));
            cards[i].transform.position = cards[i].GetComponent<Card>().position;
        }
    }
    private void Update()
    {
        counter++;
        if (counter > 50)
        {
            counter = 0;
            this.DrawCard();
        }
    }
}
