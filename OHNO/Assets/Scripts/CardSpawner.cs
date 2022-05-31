
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    private int counter = 0;
    private int sum = 0;
    private int foundtimes = 0;
    private int cardCount = 0;
    public GameObject cardObject;
    private int delay =1;

    private bool found = false;
    // Update is called once per frame
    void Update()
    {
        if (!found)
        {
            counter = counter + 1;
            if (counter%delay == 0)
            {
                cardCount++;
                //GameObject card = Instantiate(cardObject, transform.position, Quaternion.identity);
                string label = RandomCardGenerator.getRandomCard(0.2f);
                //card.GetComponent<Card>().setLabel(label);
                if (label == "y6")
                {
                    sum += cardCount;
                    foundtimes++;
                    Debug.Log("Average for " + foundtimes+ " iterations:" + sum/foundtimes);
                    cardCount = 0;
                }
            }
        }
        
    }
}
