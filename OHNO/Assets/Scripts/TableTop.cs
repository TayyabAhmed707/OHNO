using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TableTop : MonoBehaviour
{
    private static Queue<GameObject> cardQueue = new Queue<GameObject>();
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
}
