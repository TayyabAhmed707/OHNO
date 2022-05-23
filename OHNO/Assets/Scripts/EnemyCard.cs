using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    public Vector3 position = new Vector3(0f, 0f, 0f);
    public Quaternion rotation = Quaternion.identity;
    public float movementSpeed = 1f;
    public float rotationSpeed = 1f;
    public EnemyHand Hand = null;
    [SerializeField] private float floatingDistanceOnHover = 0.3f;

    public bool movingUsingCode = true;
    public bool freeFall = false;
    

    public void setPosition(Vector3 pos)
    {
        position = pos;
    }
   
    
    private void UpdateCardPositionAndRotation()
    {
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    private void Update()
    {
        if (!movingUsingCode && Vector3.Distance(transform.position,position) < 0.1f)
        {
            
            freeFall = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        
        if (!freeFall && (transform.position != position || transform.rotation != rotation))
        {
            UpdateCardPositionAndRotation();
        }
        
        
   
    }

    void playCard()
    {
        
    }
    
}
